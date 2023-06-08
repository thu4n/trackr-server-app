namespace TestTestServer.Models;

public class BlobRequestDTo
{
    public BlobRequestDTo()
    {
        Blob = new BlobDTo();
    }
    public BlobDTo Blob { get; set; }
    public string? status { get; set; }
    public bool? Error { get; set;}
}
