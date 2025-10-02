namespace WebApplication1.Contracts.Media
{
    public class MediaResponse
    {
        public Guid Id { get; set; }
        public Guid? AlbumId { get; set; }
        public string Url { get; set; } = null!;
        public string Type { get; set; } = null!;
        public Guid? UploadedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
