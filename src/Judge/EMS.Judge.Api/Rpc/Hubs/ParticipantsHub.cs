using Core.Application.Rpc.Procedures;
using Core.Domain.AggregateRoots.Manager;
using Core.Domain.AggregateRoots.Manager.Aggregates.Participants;
using EMS.Judge.Api.Configuration;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EMS.Judge.Api.Rpc.Hubs;

public class ParticipantsHub : Hub<IParticipantsClientProcedures>, IParticipantstHubProcedures
{
    private readonly ManagerRoot managerRoot;
    
    public ParticipantsHub(IJudgeServiceProvider provider)
    {
        this.managerRoot = provider.GetRequiredService<ManagerRoot>();
    }

    public IEnumerable<ParticipantEntry> Get()
    {
        return this.managerRoot.GetActiveParticipants();
    }
    public Task Witness(IEnumerable<ParticipantEntry> entries, WitnessEventType type)
    {
        foreach (var entry in entries)
        {
            var witnessEvent = new WitnessEvent
            {
                TagId = entry.Number,
                Time = entry.ArriveTime!.Value,
                Type = type,
                IsFromWitnessApp = true,
            };
            Core.Domain.AggregateRoots.Manager.WitnessEvents.Witness.Raise(witnessEvent);
        }
        return Task.CompletedTask;
    }
}
