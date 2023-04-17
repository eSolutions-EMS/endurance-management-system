using Endurance.Judge.Gateways.API.Requests;
using Endurance.Judge.Gateways.API.Services;
using Microsoft.AspNetCore.Mvc;
using static EnduranceJudge.Application.ApplicationConstants;
namespace Endurance.Judge.Gateways.API.Controllers
{
    [ApiController]
    [Route(Api.STARTLIST)]
    public class StartlistController : ControllerBase
    {
        private readonly IStartlistStateService startlistStateService;
        public StartlistController(IStartlistStateService startlistStateService)
        {
            this.startlistStateService = startlistStateService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var startlist = this.startlistStateService.Get();
            return this.Ok(startlist);
        }

        [HttpPost]
        public IActionResult Post([FromBody] StartlistRequest request)
        {
            this.startlistStateService.Set(request.Startlist);
            return this.Ok();
        }  
    }
}
