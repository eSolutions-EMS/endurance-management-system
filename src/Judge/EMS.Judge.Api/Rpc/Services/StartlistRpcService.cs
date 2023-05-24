using Core.Application.Rpc.Procedures;
using Core.Domain.AggregateRoots.Manager;
using Core.Domain.AggregateRoots.Manager.WitnessEvents;
using Core.Enums;
using EMS.Judge.Api.Configuration;
using EMS.Judge.Api.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace EMS.Judge.Api.Rpc.Services;

public class StartlistRpcService : IClientRpcService
{
    private readonly ManagerRoot managerRoot;
    private readonly IHubContext<StartlistHub, IStartlistClientProcedures> startlistHub;
    
    public StartlistRpcService(
        IHubContext<StartlistHub, IStartlistClientProcedures> startlistHub,
        IJudgeServiceProvider judgeServiceProvider)
    {
        this.startlistHub = startlistHub;
        this.managerRoot = judgeServiceProvider.GetRequiredService<ManagerRoot>();
        Witness.StartlistChanged += (_, args) => this.Update(args.number, args.action);
    }
    
    public void Update(string number, CollectionAction action)
    {
        var entry = this.managerRoot.GetStarlistEntry(number);
        if (entry == null)
        {
            return;
        }
        this.startlistHub.Clients.All.Update(entry, action);
    }
}
