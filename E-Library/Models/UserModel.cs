namespace E_Library.Models
{
    public class UserModel
    {
        public Guid id;
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty; 
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
