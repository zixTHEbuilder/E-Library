namespace E_Library.Services
{
    public interface ITestService
    {
        Task<string> GetDataAsync();
        Task<int> GetNumberAsync();
    }
    
}
