namespace E_Library.Dtos
{
    public class PagedResult<T>
    {
        public int CurentPage;
        public int TotalPages;
        public List<T> Data = new List<T>();
        public int TotalBooks;
    }
}
