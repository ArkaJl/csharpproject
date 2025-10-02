namespace WebApplication1.Contracts.User
{
    public class CreateUserRequest
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? AvatarUrl { get; set; }
        public string? Status { get; set; }
        public string? ThemePreference { get; set; } = "light";
    }
}
