namespace WebApplication1.Contracts.Album
{
    public class AlbumResponse
    {
        public Guid Id { get; set; }
        public Guid? CommunityId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
