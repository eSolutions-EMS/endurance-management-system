using Endurance.Judge.Gateways.API.Requests;
using Endurance.Judge.Gateways.API.Services;
using EnduranceJudge.Domain.AggregateRoots.Manager;
using Microsoft.AspNetCore.Mvc;
using static EnduranceJudge.Application.ApplicationConstants;

namespace Endurance.Judge.Gateways.API.Controllers
{
    [ApiController]
    [Route(Api.WITNESS)]
    public class WitnessController : ControllerBase
    {
        private readonly IStateEventService stateEventService;
        public WitnessController(IStateEventService stateEventService)
        {
            this.stateEventService = stateEventService;
        }
        
        [HttpPost]
        public IActionResult Post([FromBody] WitnessRequest request)
        {
            this.stateEventService.AddEvent(request);
            return this.Ok();
        }
        
        // Backwards compatibility for Android Handheld devices
        [HttpPost("vet")]
        public IActionResult Vet([FromBody] TagRequest request)
        {
            this.stateEventService.AddEvent(WitnessEventType.VetIn, request);
            return this.Ok();
        }
        [HttpPost("finish")]
        public IActionResult Finish([FromBody] TagRequest request)
        {
            this.stateEventService.AddEvent(WitnessEventType.Arrival, request);
            return this.Ok();
        }
    }
}
