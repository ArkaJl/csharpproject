namespace WebApplication1.Contracts.Post
{
    public class PostResponse
    {
        public Guid Id { get; set; }
        public Guid? AuthorId { get; set; }
        public Guid? CommunityId { get; set; }
        public string Content { get; set; } = null!;
        public string? Images { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Visibility { get; set; } = null!;
    }
}
