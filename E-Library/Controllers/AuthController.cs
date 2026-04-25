using E_Library.Dtos;
using E_Library.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;

namespace E_Library.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class AuthController(IAuthService authservice) : ControllerBase
    {
        private readonly IAuthService _auth = authservice;

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync(UserDto request)
        { 
            if (NullOrEmptyChecker(request.Username, request.Password)) return BadRequest("Username or Password cannot be empty");

            var register = await _auth.RegisterAsync(request);
            if (register == null) return Conflict("User already exists");

            return Ok("Account Created");

        }
        [HttpPost("Login")]
        public async Task<ActionResult<TokenResponseDto>> LoginAsync(UserDto request)
        {

        }
        [HttpPost("RefreshToken")]
        public async Task<ActionResult<TokenResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto)
        {

        }
        private bool NullOrEmptyChecker(params string[] values) => values.Any(string.IsNullOrEmpty);
    }
}
