namespace WebApplication1.Contracts.Post
{
    public class UpdatePostRequest
    {
        public string? Content { get; set; }
        public string? Images { get; set; }
        public string? Visibility { get; set; }
    }
}
