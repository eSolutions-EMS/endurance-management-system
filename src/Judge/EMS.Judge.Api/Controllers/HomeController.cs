using EMS.Judge.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Judge.Api.Controllers
{
    [ApiController]
    [Route("/")]
    public class HomeController : ControllerBase
    {
        private readonly INetwork network;
        public HomeController(INetwork network)
        {
            this.network = network;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            var ip = this.network.GetIpAddress();
            var content = $"IP: {ip}";
            return this.Ok(content);
        }
    }
}
