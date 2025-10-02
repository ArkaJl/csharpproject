namespace WebApplication1.Contracts.Message
{
    public class MessageResponse
    {
        public Guid Id { get; set; }
        public Guid ChatId { get; set; }
        public Guid SenderId { get; set; }
        public string Content { get; set; } = null!;
        public bool ReadStatus { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
