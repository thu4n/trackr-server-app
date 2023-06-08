using TestTestServer.Data;
using Microsoft.AspNetCore.Mvc;
using TestTestServer.Models;
using Microsoft.EntityFrameworkCore;
using TestTestServer;

namespace TestTestServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly FileService _fileService;
        public ImageController(FileService fileService) 
        {
            _fileService = fileService;
        }
        [HttpPost]
        public async Task<IActionResult> UploadImages(IFormFile File)
        {
            var result = await _fileService.UpLoadAsync(File);
            return Ok(result);
        }
    }
}
