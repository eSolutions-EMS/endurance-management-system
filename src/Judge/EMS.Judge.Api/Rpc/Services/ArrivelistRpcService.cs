using Core.Application.Rpc.Procedures;
using Core.Domain.AggregateRoots.Manager;
using Core.Domain.AggregateRoots.Manager.WitnessEvents;
using Core.Enums;
using EMS.Judge.Api.Configuration;
using EMS.Judge.Api.Rpc.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace EMS.Judge.Api.Rpc.Services;

public class ArrivelistRpcService : IClientRpcService
{
    private readonly ManagerRoot managerRoot;
    private readonly IHubContext<ArrivelistHub, IArrivelistClientProcedures> hubContext;
    
    public ArrivelistRpcService(
        IJudgeServiceProvider provider, 
        IHubContext<ArrivelistHub, IArrivelistClientProcedures> hubContext)
    {
        this.managerRoot = provider.GetRequiredService<ManagerRoot>();
        this.hubContext = hubContext;
        Witness.ArrivelistChanged += (sender, args) => this.Update(args.number, args.action);
    }

    public void Update(string number, CollectionAction action)
    {
        var entry = this.managerRoot.GetArrivelistEntry(number);
        if (entry == null)
        {
            return;
        }
        this.hubContext.Clients.All.Update(entry, action);
    }
}
