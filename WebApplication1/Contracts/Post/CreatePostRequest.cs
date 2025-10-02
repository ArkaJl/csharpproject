namespace WebApplication1.Contracts.Post
{
    public class CreatePostRequest
    {
        public Guid AuthorId { get; set; }
        public Guid CommunityId { get; set; }
        public string Content { get; set; } = null!;
        public string? Images { get; set; }
        public string Visibility { get; set; } = "public";
    }
}
