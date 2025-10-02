namespace WebApplication1.Contracts.User
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? AvatarUrl { get; set; }
        public int? Coins { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastOnline { get; set; }
        public string? ThemePreference { get; set; }
    }
}
