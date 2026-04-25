namespace E_Library.Dtos
{
    public class BookDto
    {
        public int Id { get; set; }
        public int PurchasePrice { get; set; }
        public string BookName { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
    }
}