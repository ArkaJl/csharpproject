namespace WebApplication1.Contracts.Transaction
{
    public class CreateTransactionRequest
    {
        public Guid UserId { get; set; }
        public int Amount { get; set; }
        public Guid? ItemId { get; set; }
    }
}
