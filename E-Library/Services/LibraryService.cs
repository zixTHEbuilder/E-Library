using E_Library.Data;
using E_Library.Dtos;
using E_Library.Migrations;
using E_Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Immutable;
using System.Xml;
using static System.Reflection.Metadata.BlobBuilder;

namespace E_Library.Services
{
    public class LibraryService(LibraryContext library) : ILibraryService
    {
        private readonly LibraryContext _library = library;

        public async Task<PagedResult<BookDisplayModel>?> GetAllBooksAsync(int pageNumber, int pageSize)
        {
            var totalBooks = await _library.Books.CountAsync();
            var books = await _library.Books.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            if (books is null) return null;

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

            if (!hasPurchased) return null;
            displayBook.BookAccessCode = book.BookAccessCode;

            return displayBook;
        }
        public async Task<BookDisplayModel?> GetByAuthorAsync(string author)
        {
            var authorBooks = await _library.Books.FirstOrDefaultAsync(a => a.Author == author);

            if (authorBooks is null) return null;

            var displayBook = new BookDisplayModel(authorBooks);
            return displayBook;
        }
        public async Task<bool?> PurchaseBookAsync(int bookId, Guid userId)
        {
            var user = await _library.User.FindAsync(userId);
            var book = await _library.Books.FindAsync(bookId);

            if (user is null || book is null) return false;

            var alreadyOwned = await _library.UserBooks
                .AnyAsync(ub => ub.UserId == userId && ub.BookId == bookId);

            if (book.PurchasePrice > user.BookCredits) return false;

            var purchaseBook = (user.BookCredits - book.PurchasePrice);
            user.BookCredits = purchaseBook;

            var addToUserBook = new UserBooks
            {
                UserId = userId,
                BookId = bookId
            };

            _library.UserBooks.Add(addToUserBook);
            await _library.SaveChangesAsync();
            return true;
        }
        public async Task<BookContent?> ReadBookAsync(int id, string accessCode)
        {
            var content = await _library.BookContent.FirstOrDefaultAsync(c=> c.id == id);

            if (content is null || content.RequiredAccessCode != accessCode) return null;

            return content;
        }
        public async Task CreateBook(CreateBookDto dto)
        {
            var book = new BookModel
            {
                BookName = dto.BookName,
                Author = dto.Author,
                PurchasePrice = dto.PurchasePrice
            };
            _library.Books.Add(book);
            await _library.SaveChangesAsync();

            var bookCreate = new BookContent
            {
                bookId = book.Id,
                title = book.BookName,
                content = dto.Body,
                RequiredAccessCode = book.BookAccessCode
            };
            _library.BookContent.Add(bookCreate);
            await _library.SaveChangesAsync();
        }
    }
}
