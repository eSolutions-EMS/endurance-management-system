using API.Requests;
using System;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get([FromBody] TagRequest tag)
        {
            Console.WriteLine($"Received: {tag.Id}");
            return this.Ok();
        }
    }
}
