namespace WebApplication1.Contracts.Transaction
{
    public class TransactionResponse
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public int Amount { get; set; }
        public string Type { get; set; } = null!;
        public Guid? ItemId { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
