using Core.Application.Rpc;
using Core.Application.Rpc.Procedures;
using Core.Domain.AggregateRoots.Manager;
using Core.Domain.AggregateRoots.Manager.Aggregates.Participants;
using Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using Core.Domain.AggregateRoots.Manager.WitnessEvents;
using EMS.Judge.Api.Configuration;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Judge.Api.Rpc.Hubs;

public class StartlistHub : Hub<IClientProcedures>, IStartlistHubProcedures, IParticipantstHubProcedures
{
	private readonly ManagerRoot managerRoot;

	public StartlistHub(IJudgeServiceProvider provider)
	{
		this.managerRoot = provider.GetRequiredService<ManagerRoot>();
	}
		
	public Dictionary<int, Startlist> SendStartlist()
	{
		var startlist = this.managerRoot.GetStartList();
		return startlist;
	}

    public ParticipantsPayload SendParticipants()
    {
        var participants = this.managerRoot.GetActiveParticipants();
        var eventId = managerRoot.GetEventId();
        return new ParticipantsPayload
        {
            Participants = participants.ToList(),
            EventId = eventId,
        };
    }
    public Task ReceiveWitnessEvent(IEnumerable<ParticipantEntry> entries, WitnessEventType type)
    {
        // Task.Run because Event hadling in dotnet seems to hold the current thread. Further investigation is needed
        // but what was happening is that Witness apps didn't receive rpc response untill the handling thread was finished
        // which is motly visible when it causes a validation (popup) which blocks the thread until closed in Prism/WPF
        Task.Run(() =>
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
                Witness.Raise(witnessEvent);
            }
        });

        return Task.CompletedTask;
    }

    public override Task OnConnectedAsync()
	{
		Console.WriteLine($"Connected: {this.Context.ConnectionId}");
        return Task.CompletedTask;
	}

	public override Task OnDisconnectedAsync(Exception exception)
	{
		Console.WriteLine($"Disconnected: {this.Context.ConnectionId}");
        return Task.CompletedTask;
    }
}
