using System.ComponentModel.DataAnnotations;

namespace E_Library.Dtos
{
    public class CreateBookDto : BookDto
    {
        [Required(ErrorMessage = "Please enter the Contents of the book")]
        public string Body { get; set; } = string.Empty;
    }
}
