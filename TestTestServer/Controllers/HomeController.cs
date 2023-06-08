using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestTestServer.Data;

namespace TestTestServer.Controllers
{
    [ApiController]
    [Route("/")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("you are not permitted access to here");
        }
    }
}