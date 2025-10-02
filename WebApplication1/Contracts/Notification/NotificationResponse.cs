namespace WebApplication1.Contracts.Notification
{
    public class NotificationResponse
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string Type { get; set; } = null!;
        public Guid? SourceId { get; set; }
        public bool IsRead { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

}
