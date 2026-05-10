namespace E_Library.Dtos
{
    public class UpdateBookDto
    {
        public int bookId { get; set; }
        public string BookName { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int PurchasePrice { get; set; }
        public string Body { get; set; } = string.Empty;
    }
}