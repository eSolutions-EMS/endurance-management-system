using Endurance.Judge.Gateways.API.Requests;
using Endurance.Judge.Gateways.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endurance.Judge.Gateways.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WitnessController : ControllerBase
    {
        private readonly IJudge judge;
        public WitnessController(IJudge judge)
        {
            this.judge = judge;
        }
        
        [HttpPost("vet")]
        public IActionResult Vet([FromBody] TagRequest request)
        {
            this.judge.EnterVet(request);
            return this.Ok();
        }
        
        [HttpPost("finish")]
        public IActionResult Finish([FromBody] TagRequest request)
        {
            this.judge.Finish(request);
            return this.Ok();
        }
    }
}
