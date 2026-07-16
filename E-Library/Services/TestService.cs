namespace E_Library.Services
{
    public class TestService : ITestService
    {
        public async Task<string> GetDataAsync()
        {
            return "Real - Data";
        }
        public async Task<int> GetNumberAsync()
        {
            return 67;
        }
    }
}
