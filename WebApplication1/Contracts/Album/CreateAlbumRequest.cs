namespace WebApplication1.Contracts.Album
{
    public class CreateAlbumRequest
    {
        public Guid CommunityId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
