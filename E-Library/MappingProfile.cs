using E_Library.Models;
namespace E_Library
{
    public static class MappingExtensions
    {
        public static List<BookDisplayModel> ToDisplay(this List<BookModel> books)
            => books.Select(b => new BookDisplayModel(b)).ToList();
    }
}
