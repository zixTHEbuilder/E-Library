using System.Security.Claims;

namespace E_Library.Dtos
{
    public class ReadBookDto
    {
        public int bookId { get; set; }
        public string accessToken { get; set; } = string.Empty;
    }
}
