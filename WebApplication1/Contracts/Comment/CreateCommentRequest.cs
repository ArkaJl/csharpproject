namespace WebApplication1.Contracts.Comment
{
    public class CreateCommentRequest
    {
        public Guid PostId { get; set; }
        public Guid AuthorId { get; set; }
        public string Text { get; set; } = null!;
    }
}
