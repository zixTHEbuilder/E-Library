using System.ComponentModel.DataAnnotations;

namespace E_Library.Dtos
{
    public class BookDto
    {
        public int id { get; set; }
        public string BookName { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int PurchasePrice { get; set; }
    }
}