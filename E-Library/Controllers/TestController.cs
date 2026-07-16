using E_Library.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace E_Library.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class TestController(ITestService testService) : ControllerBase
    {
        private readonly ITestService _test = testService;

        [HttpGet("test")]
        public async Task<IActionResult> GetRealData()
        {
            var data = await _test.GetDataAsync();
            var number = await _test.GetNumberAsync();
            return Ok(new {data,number});
        }
        [HttpGet("mock")]
        public async Task<IActionResult> GetMockData()
        {
            var mockTest = new Mock<ITestService>();

            mockTest.Setup(t => t.GetDataAsync()).ReturnsAsync("Mock - Data");
            mockTest.Setup(t => t.GetNumberAsync()).ReturnsAsync(123);

            var data = await mockTest.Object.GetDataAsync();
            var number = await mockTest.Object.GetNumberAsync();

            return Ok(new {data,number});
        }
    }
}
