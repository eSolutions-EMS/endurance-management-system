using Microsoft.AspNetCore.SignalR;
using Not.Application.Ports.CRUD;
using NTS.Domain.Core.Aggregates.Participations;
using NTS.Domain.Core.Entities;
using NTS.Judge.MAUI.Server.ACL.EMS;
using NTS.Judge.MAUI.Server.ACL.Factories;

namespace NTS.Judge.MAUI.Server.ACL;

public class EmsRpcHub : Hub<IEmsClientProcedures>, IEmsStartlistHubProcedures, IEmsEmsParticipantstHubProcedures
{
    private readonly IRepository<Participation> _participations;
    private readonly IRepository<Event> _events;

    public EmsRpcHub(IRepository<Participation> participations, IRepository<Event> events)
    {
        _participations = participations;
        _events = events;
    }

    public Dictionary<int, EmsStartlist> SendStartlist()
    {
        var participations = _participations.ReadAll().Result;
        var emsParticipations = participations
            .Select(EmsParticipationFactory.Create);
        
        var startlists = new Dictionary<int, EmsStartlist>();
        foreach (var emsParticipation in emsParticipations)
        {
            var toSkip = 0;
            foreach (var record in emsParticipation.Participant.LapRecords.Where(x => x.Result == null || !x.Result.IsNotQualified))
            {
                var entry = new EmsStartlistEntry(emsParticipation, toSkip);
                if (startlists.ContainsKey(entry.Stage))
                {
                    startlists[entry.Stage].Add(entry);
                }
                else
                {
                    startlists.Add(entry.Stage, new EmsStartlist(new List<EmsStartlistEntry> { entry }));
                }
                // If record is complete, but is not last -> insert another record for current stage
                // This bullshit happens because "current" stage is not yet created. Its only created at Arrive
                if (record == emsParticipation.Participant.LapRecords.Last() && record.NextStarTime.HasValue)
                {
                    var nextEntry = new EmsStartlistEntry(emsParticipation)
                    {
                        Stage = emsParticipation.Participant.LapRecords.Count + 1,
                        StartTime = record.NextStarTime.Value,
                    };
                    if (startlists.ContainsKey(nextEntry.Stage))
                    {
                        startlists[nextEntry.Stage].Add(nextEntry);
                    }
                    else
                    {
                        startlists.Add(nextEntry.Stage, new EmsStartlist(new List<EmsStartlistEntry> { nextEntry }));
                    }
                }
                toSkip++;
            }
        }
        return startlists;
    }

    public EmsParticipantsPayload SendParticipants()
    {
        var participants = _participations
            .ReadAll(x => x.Phases.All(x => x.ArriveTime == null || x.InspectTime == null|| x.IsReinspectionRequested && x.ReinspectTime == null))
            .Result
            .Select(EmsParticipantEntryFactory.Create);
        var @event = _events.Read(0);
        return new EmsParticipantsPayload
        {
            Participants = participants.ToList(),
            EventId = @event.Id,
        };
    }
    public Task ReceiveWitnessEvent(IEnumerable<EmsParticipantEntry> entries, EmsWitnessEventType type)
    {
        // Task.Run because Event hadling in dotnet seems to hold the current thread. Further investigation is needed
        // but what was happening is that Witness apps didn't receive rpc response untill the handling thread was finished
        // which is motly visible when it causes a validation (popup) which blocks the thread until closed in Prism/WPF
        Task.Run(() =>
        {
            foreach (var entry in entries)
            {
                //TODO: raise Participation event
            }
        });

        return Task.CompletedTask;
    }

    public override Task OnConnectedAsync()
    {
        Console.WriteLine($"Connected: {Context.ConnectionId}");
        return Task.CompletedTask;
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        Console.WriteLine($"Disconnected: {Context.ConnectionId}");
        return Task.CompletedTask;
    }

    public class ClientService : IDisposable
    {
        private readonly IHubContext<EmsRpcHub, IEmsClientProcedures> _hub;

        public ClientService(
            IHubContext<EmsRpcHub, IEmsClientProcedures> hub)
        {
            _hub = hub;
            // TODO attach event handlers
        }

        public void SendStartlistEntryUpdate(object? _, (string Number, EmsCollectionAction Action) args)
        {
            //var entry = this._managerRoot.GetStarlistEntry(args.Number);
            if (entry == null)
            {
                return;
            }
            _hub.Clients.All.ReceiveEntry(entry, args.Action);
        }

        public void SendParticipantEntryUpdate(object? _, (string Number, EmsCollectionAction Action) args)
        {
            //var entry = this._managerRoot.GetParticipantEntry(args.Number);
            if (entry == null)
            {
                return;
            }
            _hub.Clients.All.ReceiveEntryUpdate(entry, args.Action);
        }

        public void Dispose()
        {
            // TODO: Dispose
        }
    }
}