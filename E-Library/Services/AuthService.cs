using E_Library.Data;
using E_Library.Dtos;
using E_Library.Models;
using E_Library.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.CodeDom.Compiler;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace E_Library.Services
{
    public class AuthService(LibraryContext _usercontext, IConfiguration configuration) : IAuthService
    {
        private readonly LibraryContext user = _usercontext;
        public async Task<UserModel?> RegisterAsync(UserDto request)
        {
            if (_usercontext.User.Any(u => u.Username == request.Username)) return null;

            var user = new UserModel();
            var hashedPassword = new PasswordHasher<UserModel>()
                .HashPassword(user, request.Password);

            user.Username = request.Username;
            user.PasswordHash = hashedPassword;

            await _usercontext.User.AddAsync(user);

            await _usercontext.SaveChangesAsync();

            return user;
        }
        public async Task<TokenResponseDto?> LoginAsync(UserDto request)
        {
            var user = await _usercontext.User.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null) return null;

            if (new PasswordHasher<UserModel>()
                .VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed) return null;

            return await CreateTokenResponse(user);            
        }

        Task<UserModel?> IAuthService.RefreshTokensAsync(RefreshTokenRequestDto request)
        {
            throw new NotImplementedException();
        }

        private async Task<TokenResponseDto> CreateTokenResponse(UserModel user)
        {
            return new TokenResponseDto
            {
                AccessToken = await CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
            };
        }

        private async Task<string> CreateToken(UserModel user)  //check if there's an error because i'm returning a string
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };
            //INSTALL : System.identityModel.Tokens.Jwt
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("jwtsettings:Token")!));

            var creds = new Microsoft.IdentityModel.Tokens.SigningCredentials(key, SecurityAlgorithms.HmacSha512); //choose the encryption type

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("jwtsettings:Issuer"),
                audience: configuration.GetValue<string>("jwtsettings:Audience"),
                claims: claims,
                expires: DateTime.Now.AddHours(67),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new Byte[32];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }
        private async Task<string> GenerateAndSaveRefreshTokenAsync(UserModel user)
        {
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            await _usercontext.SaveChangesAsync();
            return refreshToken;
        }

    }
}
