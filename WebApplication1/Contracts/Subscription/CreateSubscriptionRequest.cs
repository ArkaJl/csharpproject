namespace WebApplication1.Contracts.Subscription
{
    public class CreateSubscriptionRequest
    {
        public Guid UserId { get; set; }
        public Guid CommunityId { get; set; }
        public string Role { get; set; } = "member";
    }
}
