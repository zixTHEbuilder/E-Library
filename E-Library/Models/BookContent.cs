using System.Runtime.CompilerServices;

namespace E_Library.Models
{
    public class BookContent
    {
        public int id { get; set; }
        public int bookId { get; set; }
        public string title { get; set; } = string.Empty;
        public string content { get; set; } = string.Empty;
        public string RequiredAccessCode { get; set; } = string.Empty;
    }
}
