namespace WebApplication1.Contracts.Auth
{
    public class AuthResponse
    {
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public DateTime Expiration { get; set; }
        public UserDto User { get; set; } = null!;
    }
}