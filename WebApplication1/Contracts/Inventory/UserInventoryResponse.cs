namespace WebApplication1.Contracts.Inventory
{
    public class UserInventoryResponse
    {
        public Guid UserId { get; set; }
        public Guid ItemId { get; set; }
        public DateTime PurchasedAt { get; set; }
        public bool IsEquipped { get; set; }
    }
}
