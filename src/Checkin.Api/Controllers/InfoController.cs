using Microsoft.AspNetCore.Mvc;

namespace Checkin.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InfoController : ControllerBase
    {
        [HttpGet]
        public IActionResult getInfo()
        {

            var info = new
            {
                Developer = new
                {
                    Name = "Pedro Serôdio",
                    Email = "serodiomg@gmail.com"
                },
                Project = new
                {
                    Name = "Checkin",
                    Description = "API para gerenciamento de eventos, check-in via QR Code e notificações",
                    Version = "v0.1"
                },
                Timestamp = DateTime.UtcNow
            };

            return Ok(info);
        }
        
    }
}
