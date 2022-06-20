using Endurance.Judge.Gateways.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endurance.Judge.Gateways.API.Controllers
{
    [ApiController]
    [Route("[controller]/events")]
    public class JudgeController : ControllerBase
    {
        private readonly IStateEventService stateEventService;
        public JudgeController(IStateEventService stateEventService)
        {
            this.stateEventService = stateEventService;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            var events = this.stateEventService.GetEvents();
            return this.Ok(events);
        }
    }
}
