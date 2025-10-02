namespace WebApplication1.Contracts.Community
{
    public class CommunityResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? AvatarUrl { get; set; }
        public string? BannerUrl { get; set; }
        public Guid? CreatorId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public object? Tags { get; set; }
        public int? MemberCount { get; set; }
    }
}
