using EMS.Judge.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Judge.Api.Controllers
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
