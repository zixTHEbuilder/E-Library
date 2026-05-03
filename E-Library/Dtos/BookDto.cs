using System.ComponentModel.DataAnnotations;

namespace E_Library.Dtos
{
    public class BookDto
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Please enter the name of the Book")]
        public string BookName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the Author")]
        public string Author { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the Purchase Price")]
        [Range(1, int.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public int PurchasePrice { get; set; }
    }
}