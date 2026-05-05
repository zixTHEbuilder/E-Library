using E_Library.Data;
using E_Library.Dtos;
using E_Library.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace E_Library.Services
{
    public class LibraryService(LibraryContext library, IServiceProvider serviceProvider) : ILibraryService
    {
        private readonly LibraryContext _library = library;
        private async Task ValidateAsync<T>(T dto)
        {
            var validator = serviceProvider.GetRequiredService<IValidator<T>>();
            await validator.ValidateAndThrowAsync(dto);
        }
        public async Task<PagedResult<BookDisplayModel>?> GetAllBooksAsync(int pageNumber, int pageSize)
        {
            var totalBooks = await _library.Books.CountAsync();
            var books = await _library.Books.OrderBy(o=>o.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            if (!books.Any()) return null;

            var displayBooks = books.Select(b => new BookDisplayModel(b)).ToList();

            return new PagedResult<BookDisplayModel>
            {
                CurentPage = pageNumber,
                TotalPages = (int)Math.Ceiling((double)totalBooks/ pageSize),
                Data = displayBooks,
                TotalBooks = totalBooks
            };
        }
        public async Task<BookDisplayModel?> GetByIdAsync(int bookId,Guid userId)
        {
            var book = await _library.Books.FindAsync(bookId);
            if (book is null) return null;

            var user = new UserModel();
            var displayBook = new BookDisplayModel(book);

            bool hasPurchased = await _library.UserBooks.AnyAsync(ub => ub.BookId == bookId && ub.UserId == userId);

            if (hasPurchased)
                displayBook.BookAccessCode = book.BookAccessCode;

            return displayBook;
        }
        public async Task<IEnumerable<BookDisplayModel>?> GetByAuthorAsync(string author)
        {
            var authorBooks = await _library.Books
                .Where(a => a.Author == author)
                .Select(book => new BookDisplayModel(book)).ToListAsync();


            if (authorBooks.Count is 0) return null;

            return authorBooks;
        }
        public async Task<bool?> PurchaseBookAsync(int bookId, Guid userId)
        {
            var user = await _library.User.FindAsync(userId);
            var book = await _library.Books.FindAsync(bookId);

            if (user is null || book is null) return false;

            var alreadyOwned = await _library.UserBooks
                .AnyAsync(ub => ub.UserId == userId && ub.BookId == bookId);
            if (alreadyOwned) return false;

            if (book.PurchasePrice > user.BookCredits) return false;

            var purchaseBook = (user.BookCredits - book.PurchasePrice);
            user.BookCredits = purchaseBook;
            user.BooksOwned = book.BookName;


            var addToUserBook = new UserBooks
            {
                UserId = userId,
                BookId = bookId,
                PurchasePrice = book.PurchasePrice,
                PurchaseDate = DateTime.UtcNow
            };

            _library.UserBooks.Add(addToUserBook);
            await _library.SaveChangesAsync();
            return true;
        }
        public async Task<BookContent?> ReadBookAsync(Guid userId, int bookId, string accessToken)
        {
            //Make sure you're searching via bookId and not the Primary key "id"
            var content = await _library.BookContent.FirstOrDefaultAsync(b=> b.bookId == bookId);

            var user = new UserModel();

            bool hasPurchased = await _library.UserBooks.AnyAsync(ub => ub.BookId == bookId && ub.UserId == userId);
            if (!hasPurchased) return null;

            if (content is null || content.RequiredAccessCode != accessToken) return null;

            return content;
        }
        public async Task<string> CreateBookAsync(CreateBookDto dto)
        {
            await ValidateAsync(dto);
            //we are using 2 "SaveChanges" here so if one is successful while the other isn't,
            //book will be created with no contents inside, to prevent that we use try catch block with transaction
            using var transaction = await _library.Database.BeginTransactionAsync();
            try
            {
                if (await _library.Books.AnyAsync(b => b.BookName == dto.BookName) || 
                    await _library.BookContent.AnyAsync(bc => bc.content == dto.Body))
                    return "Duplicate Request : Book with that name or content already exists";

                var book = new BookModel
                {
                    BookName = dto.BookName,
                    Author = dto.Author,
                    PurchasePrice = dto.PurchasePrice
                };
                string assignedCode = book.BookAccessCode;

                _library.Books.Add(book);
                await _library.SaveChangesAsync();

                var bookContent = new BookContent
                {
                    bookId = book.Id,
                    title = book.BookName,
                    content = dto.Body,
                    RequiredAccessCode = assignedCode
                };
                _library.BookContent.Add(bookContent);
                await _library.SaveChangesAsync();

                await transaction.CommitAsync();
                return "Success";
            }
            catch(Exception e)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Database error : {e.Message}");
                return "An unexpected error occured while adding the book, please try again later";
            }
        }
    }
}
