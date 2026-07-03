using Domain;
using Microsoft.AspNetCore.Mvc;

namespace E_Library.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class CalculatorController : ControllerBase
    {
        private readonly Calculator calculator = new Calculator();
        [HttpGet("add/{num1}/{num2}")]
        public IActionResult add(int num1, int num2)
        {
            return Ok(calculator.Add(num1, num2));
        }
    }
}
