namespace WebApplication1.Contracts.Comment
{
    public class CommentResponse
    {
        public Guid Id { get; set; }
        public Guid? PostId { get; set; }
        public Guid? AuthorId { get; set; }
        public string Text { get; set; } = null!;
        public int? LikesCount { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
