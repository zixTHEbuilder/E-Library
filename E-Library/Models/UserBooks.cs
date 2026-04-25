namespace E_Library.Models
{
    public class UserBooks
    {
        public int id { get; set; }
        public Guid UserId { get; set; }
        public int BookId { get; set; }
        public int PurchasePrice { get; set; }
        public DateTime PurchaseDate = DateTime.Now;
    }
}
