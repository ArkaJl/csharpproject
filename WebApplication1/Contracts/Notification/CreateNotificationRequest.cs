namespace WebApplication1.Contracts.Notification
{
    public class CreateNotificationRequest
    {
        public Guid UserId { get; set; }
        public string Type { get; set; } = null!;
        public Guid? SourceId { get; set; }
    }
}