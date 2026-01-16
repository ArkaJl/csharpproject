namespace WebApplication1.Contracts.Auth
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? AvatarUrl { get; set; }
        public int? Coins { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public DateTime? LastOnline { get; set; }
        public string? Status { get; set; }
    }
}