namespace WebApplication1.Contracts.Chat
{
    public class ChatResponse
    {
        public Guid Id { get; set; }
        public Guid? CommunityId { get; set; }
        public string? Name { get; set; }
        public string Type { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
    }
}
