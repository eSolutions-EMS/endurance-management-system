using Endurance.Judge.Gateways.API.Services;
using EnduranceJudge.Application;
using Microsoft.AspNetCore.Mvc;

namespace Endurance.Judge.Gateways.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StateController : ControllerBase
    {
        private readonly IReadonlyContext context;
        private readonly IStateChangesQueue stateChangesQueue;
        private readonly IJudgeEventQueue judgeEventQueue;
        public StateController(
            IReadonlyContext context,
            IStateChangesQueue stateChangesQueue,
            IJudgeEventQueue judgeEventQueue)
        {
            this.context = context;
            this.stateChangesQueue = stateChangesQueue;
            this.judgeEventQueue = judgeEventQueue;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            this.judgeEventQueue.ExecuteEvents();
            return this.Ok(this.context.State);
        }

        [HttpPost]
        public IActionResult Post([FromBody] State state)
        {
            this.stateChangesQueue.Enqueue(state);
            return this.Ok();
        }
    }
}
