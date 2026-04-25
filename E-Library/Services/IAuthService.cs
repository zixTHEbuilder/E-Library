using E_Library.Dtos;
using E_Library.Models;

namespace E_Library.Services
{
    public interface IAuthService
    {
        Task<UserModel?> RegisterAsync(UserDto request);
        Task<TokenResponseDto?> LoginAsync(UserDto request);
        Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request);
    }
}
