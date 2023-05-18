using Core.Domain.State;
using EMS.Judge.Api.Configuration;
using EMS.Judge.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace EMS.Judge.Api.Controllers;

[ApiController]
[Route("/")]
public class HomeController : ControllerBase
{
    private readonly INetwork network;
    private readonly IJudgeServiceProvider _judgeServiceProvider;
    public HomeController(INetwork network, IJudgeServiceProvider judgeServiceProvider)
    {
        this.network = network;
        this._judgeServiceProvider = judgeServiceProvider;
    }
        
    [HttpGet]
    public IActionResult Get()
    {
        var state = this._judgeServiceProvider.GetRequiredService<IState>();
        var ip = this.network.GetIpAddress();
        var content = $"IP: {ip} - {state?.Event?.Name}";
        return this.Ok(content);
    }
}
