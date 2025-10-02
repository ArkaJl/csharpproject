namespace WebApplication1.Contracts.Message
{
    public class CreateMessageRequest
    {
        public Guid ChatId { get; set; }
        public Guid SenderId { get; set; }
        public string Content { get; set; } = null!;
    }
}
