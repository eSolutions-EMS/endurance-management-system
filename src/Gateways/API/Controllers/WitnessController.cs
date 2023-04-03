using Endurance.Judge.Gateways.API.Requests;
using Endurance.Judge.Gateways.API.Services;
using EnduranceJudge.Domain.AggregateRoots.Manager;
using Microsoft.AspNetCore.Mvc;

namespace Endurance.Judge.Gateways.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WitnessController : ControllerBase
    {
        private readonly IStateEventService stateEventService;
        public WitnessController(IStateEventService stateEventService)
        {
            this.stateEventService = stateEventService;
        }
        
        [HttpPost("vet")]
        public IActionResult Vet([FromBody] TagRequest request)
        {
            this.stateEventService.AddEvent(WitnessEventType.EnterVet, request);
            return this.Ok();
        }
        
        [HttpPost("finish")]
        public IActionResult Finish([FromBody] TagRequest request)
        {
            this.stateEventService.AddEvent(WitnessEventType.Finish, request);
            return this.Ok();
        }
    }
}
