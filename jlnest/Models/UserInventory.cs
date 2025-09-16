using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class UserInventory
{
    public Guid UserId { get; set; }

    public Guid ItemId { get; set; }

    public DateTime? PurchasedAt { get; set; }

    public bool? IsEquipped { get; set; }

    public virtual StoreItem Item { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
