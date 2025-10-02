using Domain.Models;

public class UserInventory
{
    public Guid UserId { get; set; }
    public Guid ItemId { get; set; }
    public DateTime PurchasedAt { get; set; }
    public bool IsEquipped { get; set; }

    // Навигационные свойства (опционально)
    public virtual User? User { get; set; }
    public virtual StoreItem? Item { get; set; }
}