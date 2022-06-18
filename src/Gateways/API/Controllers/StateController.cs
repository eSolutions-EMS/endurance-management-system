using Endurance.Judge.Gateways.API.Services;
using EnduranceJudge.Application;
using EnduranceJudge.Domain.State;
using Microsoft.AspNetCore.Mvc;

namespace Endurance.Judge.Gateways.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StateController : ControllerBase
    {
        private readonly IReadonlyContext context;
        private readonly IStateChangesQueue stateChangesQueue;
        public StateController(IReadonlyContext context, IStateChangesQueue stateChangesQueue)
        {
            this.context = context;
            this.stateChangesQueue = stateChangesQueue;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            var raw = this.context.State;
            return this.Ok(raw);
        }

        [HttpPost]
        public IActionResult Post([FromBody] State state)
        {
            this.stateChangesQueue.Enqueue(state);
            return this.Ok();
        }
    }
}
