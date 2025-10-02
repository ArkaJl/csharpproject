namespace WebApplication1.Contracts.Media
{
    public class UploadMediaRequest
    {
        public Guid? AlbumId { get; set; }
        public string Url { get; set; } = null!;
        public string Type { get; set; } = null!;
        public Guid UploadedBy { get; set; }
    }
}
