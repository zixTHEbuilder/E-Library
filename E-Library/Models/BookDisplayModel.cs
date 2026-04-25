namespace E_Library.Models
{
    public class BookDisplayModel
    {
        public int Id { get; set; }
        public int PurchasePrice { get; set; }
        public string BookName { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string BookAccessCode { get; set; } = "Purchase the book to get the Token";

        public BookDisplayModel() { }
        public BookDisplayModel(BookModel book)
        {
            Id = book.Id;
            PurchasePrice = book.PurchasePrice;
            BookName = book.BookName;
            Author = book.Author;
        }
    }
}
