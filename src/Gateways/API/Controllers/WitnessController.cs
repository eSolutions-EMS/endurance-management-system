using Endurance.Judge.Gateways.API.Models;
using Endurance.Judge.Gateways.API.Requests;
using Endurance.Judge.Gateways.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endurance.Judge.Gateways.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WitnessController : ControllerBase
    {
        private readonly IJudgeEventQueue judgeEventQueue;
        public WitnessController(IJudgeEventQueue judgeEventQueue)
        {
            this.judgeEventQueue = judgeEventQueue;
        }
        
        [HttpPost("vet")]
        public IActionResult Vet([FromBody] TagRequest request)
        {
            this.judgeEventQueue.AddEvent(JudgeEventType.EnterVet, request);
            return this.Ok();
        }
        
        [HttpPost("finish")]
        public IActionResult Finish([FromBody] TagRequest request)
        {
            this.judgeEventQueue.AddEvent(JudgeEventType.Finish, request);
            return this.Ok();
        }
    }
}
