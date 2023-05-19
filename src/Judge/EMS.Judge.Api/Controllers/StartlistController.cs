using EMS.Judge.Application;
using EMS.Judge.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Judge.Api.Controllers;

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
}
