using System.ComponentModel.DataAnnotations;

namespace E_Library.Dtos
{
    public class CreateBookDto : BookDto
    {
        public string Body { get; set; } = string.Empty;
    }
}
