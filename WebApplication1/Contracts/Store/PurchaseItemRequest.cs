namespace WebApplication1.Contracts.Store
{
    public class PurchaseItemRequest
    {
        public Guid UserId { get; set; }
        public Guid ItemId { get; set; }
    }
}
