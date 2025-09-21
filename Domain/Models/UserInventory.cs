using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    [Table("user_inventory")]
    public class UserInventory
    {
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [ForeignKey("StoreItem")]
        public Guid ItemId { get; set; }

        public DateTime PurchasedAt { get; set; }

        public bool IsEquipped { get; set; }

        public virtual required User User { get; set; }
        public virtual required StoreItem Item { get; set; }
    }
}