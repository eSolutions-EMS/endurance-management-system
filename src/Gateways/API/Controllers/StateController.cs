using Endurance.Judge.Gateways.API.Requests;
using Endurance.Judge.Gateways.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endurance.Judge.Gateways.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StateController : ControllerBase
    {
        private readonly IContext context;
        private readonly IStateChangesQueue stateChangesQueue;
        public StateController(IContext context, IStateChangesQueue stateChangesQueue)
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
        public IActionResult Post([FromBody] StateRequest request)
        {
            this.stateChangesQueue.Enqueue(request.State);
            return this.Ok();
        }
    }
}
