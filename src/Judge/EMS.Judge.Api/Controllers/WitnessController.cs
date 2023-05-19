using EMS.Judge.Application;
using Core.Domain.AggregateRoots.Manager;
using EMS.Judge.Api.Requests;
using EMS.Judge.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Judge.Api.Controllers;

[ApiController]
[Route(ApplicationConstants.Api.WITNESS)]
public class WitnessController : ControllerBase
{
    private readonly IWitnessEventService _witnessEventService;
    public WitnessController(IWitnessEventService witnessEventService)
    {
        this._witnessEventService = witnessEventService;
    }
        
    [HttpPost]
    public IActionResult Post([FromBody] WitnessRequest request)
    {
        this._witnessEventService.AddEvent(request);
        return this.Ok();
    }
        
    // Backwards compatibility for Android Handheld devices
    [HttpPost("vet")]
    public IActionResult Vet([FromBody] TagRequest request)
    {
        this._witnessEventService.AddEvent(WitnessEventType.VetIn, request);
        return this.Ok();
    }
    [HttpPost("finish")]
    public IActionResult Finish([FromBody] TagRequest request)
    {
        this._witnessEventService.AddEvent(WitnessEventType.Arrival, request);
        return this.Ok();
    }
}
