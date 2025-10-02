namespace WebApplication1.Contracts.Subscription
{
    public class SubscriptionResponse
    {
        public Guid UserId { get; set; }
        public Guid CommunityId { get; set; }
        public string Role { get; set; } = null!;
        public DateTime? JoinedAt { get; set; }
    }
}
