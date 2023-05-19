using EMS.Judge.Application;
using EMS.Judge.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Judge.Api.Controllers;

[ApiController]
[Route(ApplicationConstants.Api.STARTLIST)]
public class StartlistController : ControllerBase
{
    private readonly IStartlistService _startlistService;
    public StartlistController(IStartlistService startlistService)
    {
        this._startlistService = startlistService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var startlist = this._startlistService.Get();
        return this.Ok(startlist);
    }
}
