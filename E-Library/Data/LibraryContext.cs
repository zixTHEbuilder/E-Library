using E_Library.Models;
using Microsoft.EntityFrameworkCore;
namespace E_Library.Data
{
    public class LibraryContext(DbContextOptions<LibraryContext> options) : DbContext(options)
    {
        public DbSet<BookModel> BookModel => Set<BookModel>();
        public DbSet<UserModel> User => Set<UserModel>();
    }
}
