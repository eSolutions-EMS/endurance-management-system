using Endurance.Judge.Gateways.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endurance.Judge.Gateways.API.Controllers
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
