using E_Library.Models;
using Microsoft.EntityFrameworkCore;
namespace E_Library.Data
{
    public class LibraryContext(DbContextOptions<LibraryContext> options) : DbContext(options)
    {
        public DbSet<BookModel> Books => Set<BookModel>();
        public DbSet<BookDisplayModel> BooksDisplay => Set<BookDisplayModel>();
        public DbSet<UserModel> User => Set<UserModel>();
        public DbSet<UserBooks> UserBooks => Set<UserBooks>();
    }
}
