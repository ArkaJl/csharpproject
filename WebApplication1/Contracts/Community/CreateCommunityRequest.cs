namespace WebApplication1.Contracts.Community
{
    public class CreateCommunityRequest
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? AvatarUrl { get; set; }
        public string? BannerUrl { get; set; }
        public string? Tags { get; set; }
        public Guid creator_id { get; set; }
    }
}
