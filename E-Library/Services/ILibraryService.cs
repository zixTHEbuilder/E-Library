using E_Library.Dtos;
using E_Library.Models;

namespace E_Library.Services
{
    public interface ILibraryService
    {
        Task<PagedResult<BookDisplayModel>?> GetAllBooksAsync(int pageNumber, int pageSize);
        Task<BookDisplayModel?> GetByIdAsync(int bookId, Guid userId);
        Task<IEnumerable<BookDisplayModel>?> GetByAuthorAsync(string author);
        Task<bool?> PurchaseBookAsync(int bookId, Guid userId);
        Task<BookContent?> ReadBookAsync(Guid userId, int bookId,string accessCode);
        Task<string> CreateBookAsync(CreateBookDto dto);
        Task<bool> UpdateBookAsync(UpdateBookDto dto);
    }
}
