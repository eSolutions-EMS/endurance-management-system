using Endurance.Judge.Gateways.API.Services;
using EnduranceJudge.Application;
using Microsoft.AspNetCore.Mvc;

namespace Endurance.Judge.Gateways.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StateController : ControllerBase
    {
        private readonly IApiContext context;
        private readonly IJudgeEventQueue judgeEventQueue;
        private readonly IStateManager stateManager;
        public StateController(
            IApiContext context,
            IJudgeEventQueue judgeEventQueue,
            IStateManager stateManager)
        {
            this.context = context;
            this.judgeEventQueue = judgeEventQueue;
            this.stateManager = stateManager;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            this.judgeEventQueue.ExecuteEvents();
            return this.Ok(this.context.ApiState);
        }

        [HttpPost]
        public IActionResult Post([FromBody] State state)
        {
            this.stateManager.Set(state);
            return this.Ok();
        }
    }
}
