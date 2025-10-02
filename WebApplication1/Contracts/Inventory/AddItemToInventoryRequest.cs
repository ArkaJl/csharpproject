namespace WebApplication1.Contracts.Inventory
{
    public class AddItemToInventoryRequest
    {
        public Guid UserId { get; set; }
        public Guid ItemId { get; set; }
        public bool IsEquipped { get; set; } = false;
    }
}
