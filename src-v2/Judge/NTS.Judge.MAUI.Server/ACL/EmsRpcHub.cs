using Microsoft.AspNetCore.SignalR;
using Not.Application.Ports.CRUD;
using Not.Events;
using NTS.Domain.Core.Aggregates.Participations;
using NTS.Domain.Core.Entities;
using NTS.Domain.Core.Events;
using NTS.Judge.Blazor.Ports;
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
            .ReadAll(x => x.Phases.All(x => !x.IsComplete))
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
        Task.Run(async () =>
        {
            foreach (var entry in entries)
            {
                var participation = await _participations.Read(x => x.Tandem.Number == int.Parse(entry.Number));
                if (participation == null)
                {
                    continue;
                }
                var isFinal = participation.Phases.Take(participation.Phases.Count - 1).All(x => x.IsComplete);
                var snapshot = SnapshotFactory.Create(entry, type, isFinal);
                //TODO: process after behind await _participationBehind.Process(snapshot);
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
        private readonly IRepository<Participation> _participations;
        private readonly IHubContext<EmsRpcHub, IEmsClientProcedures> _hub;

        public ClientService(
            IRepository<Participation> participations,
            IHubContext<EmsRpcHub, IEmsClientProcedures> hub)
        {
            _participations = participations;
            _hub = hub;

            EventHelper.Subscribe<PhaseCompleted>(SendStartlistEntryUpdate);
            EventHelper.Subscribe<QualificationRestored>(SendParticipantEntryAddOrUpdate);
            EventHelper.Subscribe<QualificationRevoked>(SendParticipantEntryRemove);
        }

        public void SendStartlistEntryUpdate(PhaseCompleted phaseCompleted)
        {
            var participation = _participations.Read(x => x.Tandem.Number == phaseCompleted.Number).Result
                ?? throw new Exception($"Could not '{nameof(SendStartlistEntryUpdate)}'! Participation with '{phaseCompleted.Number}' not found");
            var emsParticipation = EmsParticipationFactory.Create(participation);
            var entry = new EmsStartlistEntry(emsParticipation);
            _hub.Clients.All.ReceiveEntry(entry, EmsCollectionAction.AddOrUpdate);
        }

        public void SendParticipantEntryAddOrUpdate(QualificationRestored qualificationRestored)
        {
            var participation = _participations.Read(x => x.Tandem.Number == qualificationRestored.Number).Result
                ?? throw new Exception($"Could not 'SendParticipantEntryUpdate(AddOrUpdate)'! Participation with '{qualificationRestored.Number}' not found");
            var emsParticipation = EmsParticipationFactory.Create(participation);
            var entry = new EmsParticipantEntry(emsParticipation);
            _hub.Clients.All.ReceiveEntryUpdate(entry, EmsCollectionAction.AddOrUpdate);
        }

        public void SendParticipantEntryRemove(QualificationRevoked qualificationRevoked)
        {
            var participation = _participations.Read(x => x.Tandem.Number == qualificationRevoked.Number).Result
                ?? throw new Exception($"Could not 'SendParticipantEntryUpdate(Remove)'! Participation with '{qualificationRevoked.Number}' not found");
            var emsParticipation = EmsParticipationFactory.Create(participation);
            var entry = new EmsParticipantEntry(emsParticipation);
            _hub.Clients.All.ReceiveEntryUpdate(entry, EmsCollectionAction.Remove);
        }

        public void Dispose()
        {
            // TODO: Dispose
        }
    }
}