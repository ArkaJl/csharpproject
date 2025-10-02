namespace WebApplication1.Contracts.Store
{
    public class StoreItemResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public int Price { get; set; }
        public string? Thumbnail { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
    }
}
