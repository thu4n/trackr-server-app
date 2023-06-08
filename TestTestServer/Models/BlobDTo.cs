namespace TestTestServer.Models
{
    public class BlobDTo
    {
        public string? uri { get; set; }
        public string? name { get; set; }
        public string? ContentType { get; set; }
        public Stream? content { get; set; }
    }
}
