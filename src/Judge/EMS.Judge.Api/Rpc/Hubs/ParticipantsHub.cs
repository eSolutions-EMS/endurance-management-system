using Core.Application.Rpc.Procedures;
using Core.Domain.AggregateRoots.Manager;
using Core.Domain.AggregateRoots.Manager.Aggregates.ParticipantEntries;
using Core.Domain.AggregateRoots.Manager.WitnessEvents;
using EMS.Judge.Api.Configuration;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EMS.Judge.Api.Rpc.Hubs;

public class ParticipantsHub : Hub, IParticipantstHubProcedures
{
    private readonly ManagerRoot managerRoot;
    
    public ParticipantsHub(IJudgeServiceProvider provider)
    {
        this.managerRoot = provider.GetRequiredService<ManagerRoot>();
    }

    public IEnumerable<ParticipantEntry> Get()
    {
        return this.managerRoot.GetParticipantsList();
    }
    public Task Save(IEnumerable<ParticipantEntry> entries)
    {
        foreach (var entry in entries)
        {
            var witnessEvent = new WitnessEvent
            {
                TagId = entry.Number,
                Time = entry.ArriveTime!.Value,
                Type = entry.Type,
                IsFromWitnessApp = true,
            };
            Witness.Raise(witnessEvent);
        }
        return Task.CompletedTask;
    }
}
