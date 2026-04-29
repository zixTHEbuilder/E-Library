namespace E_Library.Models
{
    public class BookModel
    {
        public int Id { get; set; }
        public int PurchasePrice { get; set; }
        public string BookName { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string BookAccessCode { get; set; } = Guid.NewGuid().ToString()[..8].ToUpper();
    }
}
