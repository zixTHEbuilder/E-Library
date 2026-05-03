namespace E_Library.Dtos
{
    public class PagedResult<T>
    {
        public int CurentPage { get; set; }
        public int TotalPages { get; set; }
        public List<T> Data { get; set; } =  new List<T>();
        public int TotalBooks { get; set; }
    }
}
