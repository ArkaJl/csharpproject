namespace WebApplication1.Contracts.User
{

    public class UpdateUserRequest
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Status { get; set; }
        public string? ThemePreference { get; set; }
    }
}
