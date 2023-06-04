using Core.Application.Rpc.Procedures;
using Core.Domain.AggregateRoots.Manager;
using Core.Domain.AggregateRoots.Manager.Aggregates.Arrivelists;
using Core.Domain.AggregateRoots.Manager.WitnessEvents;
using EMS.Judge.Api.Configuration;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EMS.Judge.Api.Rpc.Hubs;

public class ArrivelistHub : Hub<IArrivelistClientProcedures>, IArrivelistHubProcedures
{
    private readonly ManagerRoot managerRoot;
    
    public ArrivelistHub(IJudgeServiceProvider provider)
    {
        this.managerRoot = provider.GetRequiredService<ManagerRoot>();
    }

    public IEnumerable<ArrivelistEntry> Get()
    {
        return this.managerRoot.GetArrivelist();
    }
    public Task Save(IEnumerable<ArrivelistEntry> entries)
    {
        foreach (var entry in entries)
        {
            var witnessEvent = new WitnessEvent
            {
                TagId = entry.Number,
                Time = entry.ArriveTime!.Value,
                Type = entry.Type,
            };
            Witness.Raise(witnessEvent);
        }
        return Task.CompletedTask;
    }
}
