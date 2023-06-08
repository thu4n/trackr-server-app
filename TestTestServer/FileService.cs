using Azure.Storage;
using Azure.Storage.Blobs;
using TestTestServer.Models;
using Azure.Storage.Blobs.Models;
namespace TestTestServer;

public class FileService
{
    
    private readonly BlobContainerClient _filesContainer;


    public FileService()
    {
        string connectiongString = "DefaultEndpointsProtocol=https;AccountName=accountfreent106;AccountKey=hRz2S8pfxxCFP6UIuDRDTaCSANebBjKJ77PH5IhJOdvCIyzr4gQUZikxFWo8+mCTo8S/mm8Yb3js+AStHmAGng==;EndpointSuffix=core.windows.net";
        var blobServiceClient = new BlobServiceClient(connectiongString);
        _filesContainer = blobServiceClient.GetBlobContainerClient("publicuploads");
    }
    public async Task<BlobRequestDTo> UpLoadAsync(IFormFile blob)
    {
        BlobRequestDTo response = new BlobRequestDTo();
        BlobClient client = _filesContainer.GetBlobClient(blob.FileName);

        await using (Stream? data = blob.OpenReadStream()) 
        {
            await client.UploadAsync(data,new BlobHttpHeaders { ContentType = blob.ContentType });
        }

        response.status = "File Upload SuccessFully";
        response.Error = false;
        response.Blob.uri = client.Uri.ToString()   ;
        response.Blob.name = client.Name;

        return response;
    }
}
