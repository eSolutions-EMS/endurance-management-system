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
    private readonly IStateEventService stateEventService;
    public WitnessController(IStateEventService stateEventService)
    {
        this.stateEventService = stateEventService;
    }
        
    [HttpPost]
    public IActionResult Post([FromBody] WitnessRequest request)
    {
        this.stateEventService.AddEvent(request);
        return this.Ok();
    }
        
    // Backwards compatibility for Android Handheld devices
    [HttpPost("vet")]
    public IActionResult Vet([FromBody] TagRequest request)
    {
        this.stateEventService.AddEvent(WitnessEventType.VetIn, request);
        return this.Ok();
    }
    [HttpPost("finish")]
    public IActionResult Finish([FromBody] TagRequest request)
    {
        this.stateEventService.AddEvent(WitnessEventType.Arrival, request);
        return this.Ok();
    }
}