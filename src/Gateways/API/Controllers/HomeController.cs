using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("/")]
    public class HomeController : ControllerBase
    {
        private readonly ILanService lanService;
        public HomeController(ILanService lanService)
        {
            this.lanService = lanService;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            var ip = this.lanService.GetIpAddress();
            var content = $"IP: {ip}";
            return this.Ok(content);
        }
    }
}
