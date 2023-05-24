using Core.Application.Rpc.Procedures;
using Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using Core.Domain.State;
using EMS.Judge.Api.Configuration;
using EMS.Judge.Api.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Core.Enums;

namespace EMS.Judge.Api.Controllers;

[ApiController]
[Route("/")]
public class HomeController : ControllerBase
{
    private readonly IJudgeServiceProvider _judgeServiceProvider;
    private readonly IHubContext<StartlistHub, IStartlistClientProcedures> hubContext;
    public HomeController(
        IJudgeServiceProvider judgeServiceProvider,
        IHubContext<StartlistHub, IStartlistClientProcedures> hubContext)
    {
        this._judgeServiceProvider = judgeServiceProvider;
        this.hubContext = hubContext;
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

    [HttpGet("hub")]
    public async Task<IActionResult> Hub()
    {
        this.hubContext.Clients.All.Update(new StartModel
        {
            Number = "11",
            Name = "text",
            StartTime = DateTime.Now,
        }, CollectionAction.AddOrUpdate);
        return this.Ok();
    }
}
