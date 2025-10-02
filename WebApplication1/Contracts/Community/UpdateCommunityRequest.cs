namespace WebApplication1.Contracts.Community
{
    public class UpdateCommunityRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? AvatarUrl { get; set; }
        public string? BannerUrl { get; set; }
        public string? Tags { get; set; }
    }
}
