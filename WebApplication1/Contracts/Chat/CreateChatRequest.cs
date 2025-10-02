namespace WebApplication1.Contracts.Chat
{
    public class CreateChatRequest
    {
        public Guid? CommunityId { get; set; }
        public string? Name { get; set; }
        public string Type { get; set; } = "private";
    }
}
