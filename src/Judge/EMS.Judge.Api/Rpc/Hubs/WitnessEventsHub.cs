using Core.Application.Rpc.Procedures;
using Core.Domain.AggregateRoots.Manager;
using Core.Domain.AggregateRoots.Manager.WitnessEvents;
using Microsoft.AspNetCore.SignalR;

namespace EMS.Judge.Api.Rpc.Hubs;

// TODO: remove
public class WitnessEventsHub : Hub, IWitnessEventsHubProcedures
{
    public void Add(WitnessEvent witnessEvent)
    {
        Witness.Raise(witnessEvent);
    }
}
