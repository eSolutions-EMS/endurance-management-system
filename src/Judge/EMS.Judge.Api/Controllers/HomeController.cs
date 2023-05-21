using Core.Domain.State;
using EMS.Judge.Api.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace EMS.Judge.Api.Controllers;

[ApiController]
[Route("/")]
public class HomeController : ControllerBase
{
    private readonly IJudgeServiceProvider _judgeServiceProvider;
    public HomeController(IJudgeServiceProvider judgeServiceProvider)
    {
        this._judgeServiceProvider = judgeServiceProvider;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var a = IPAddress.Broadcast.ToString();
        var hostName = Dns.GetHostName();
        var ipHostInfo = Dns.GetHostEntry(hostName);
        var ip = ipHostInfo.AddressList.FirstOrDefault(x =>
            x.AddressFamily == AddressFamily.InterNetwork
            && x.ToString().StartsWith("192.168"));

        var state = this._judgeServiceProvider.GetRequiredService<IState>();
        var content = $"IP: {ip} - {state?.Event?.Name}";
        return this.Ok(content);
    }
}
