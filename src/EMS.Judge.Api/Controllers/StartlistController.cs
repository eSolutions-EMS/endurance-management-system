using EMS.Core.Application;
using EMS.Judge.Api.Requests;
using EMS.Judge.Api.Services;
using Microsoft.AspNetCore.Mvc;
using static EMS.Core.Application.ApplicationConstants;

namespace EMS.Judge.Api.Controllers
{
    [ApiController]
    [Route(ApplicationConstants.Api.STARTLIST)]
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
            this.startlistStateService.Set(request);
            return this.Ok();
        }
    }
}
