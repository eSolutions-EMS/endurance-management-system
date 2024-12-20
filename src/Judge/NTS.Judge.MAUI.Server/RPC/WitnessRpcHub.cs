using Microsoft.AspNetCore.SignalR;
using Not.Application.CRUD.Ports;
using Not.Safe;
using NTS.ACL.Entities;
using NTS.ACL.Entities.EMS;
using NTS.ACL.Factories;
using NTS.ACL.RPC;
using NTS.ACL.RPC.Procedures;
using NTS.Application.RPC;
using NTS.Domain.Core.Aggregates;
using NTS.Domain.Objects;
using NTS.Judge.MAUI.Server.RPC.Procedures;

namespace NTS.Judge.MAUI.Server.RPC;

public class WitnessRpcHub
    : Hub<IClientProcedures>,
        IEmsStartlistHubProcedures,
        IEmsEmsParticipantstHubProcedures
{
    readonly IRead<Participation> _participations;
    readonly IRead<EnduranceEvent> _events;
    readonly IHubContext<JudgeRpcHub, IJudgeClientProcedures> _judgeRelay;

    public WitnessRpcHub(
        IRead<Participation> participations,
        IRead<EnduranceEvent> events,
        IHubContext<JudgeRpcHub, IJudgeClientProcedures> judgeRelay
    )
    {
        _participations = participations;
        _events = events;
        _judgeRelay = judgeRelay;
    }

    public Dictionary<int, EmsStartlist> SendStartlist()
    {
        var participations = _participations.ReadAll(x => !x.IsEliminated()).Result;
        var emsParticipations = participations.Select(ParticipationFactory.CreateEms);

        var startlists = new Dictionary<int, EmsStartlist>();
        foreach (var emsParticipation in emsParticipations)
        {
            var toSkip = 0;
            foreach (
                var record in emsParticipation.Participant.LapRecords.Where(x =>
                    x.Result == null || !x.Result.IsNotQualified
                )
            )
            {
                var entry = new EmsStartlistEntry(emsParticipation, toSkip);
                if (startlists.ContainsKey(entry.Stage))
                {
                    startlists[entry.Stage].Add(entry);
                }
                else
                {
                    startlists.Add(entry.Stage, new EmsStartlist([entry]));
                }
                // If record is complete, but is not last -> insert another record for current stage
                // This bullshit happens because "current" stage is not yet created. Its only created at Arrive
                if (
                    record == emsParticipation.Participant.LapRecords.Last()
                    && record.NextStarTime.HasValue
                )
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
                        startlists.Add(nextEntry.Stage, new EmsStartlist([nextEntry]));
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
            .ReadAll(x => !x.IsEliminated() && !x.IsComplete())
            .Result.Select(ParticipantEntryFactory.Create);
        var enduranceEvent = _events.Read(0).Result;
        return new EmsParticipantsPayload
        {
            Participants = participants.ToList(),
            EventId = enduranceEvent?.Id ?? 0,
        };
    }

    public async Task ReceiveWitnessEvent(
        IEnumerable<EmsParticipantEntry> entries,
        EmsWitnessEventType type
    )
    {
        // Task.Run because Event hadling in dotnet seems to hold the current thread. Further investigation is needed
        // but what was happening is that Witness apps didn't receive rpc response untill the handling thread was finished
        // which is motly visible when it causes a validation (popup) which blocks the thread until closed in Prism/WPF
        await SafeHelper.RunAsync(async () =>
        {
            var snapshots = new List<Snapshot>();
            foreach (var entry in entries)
            {
                var participation = await _participations.Read(x =>
                    x.Combination.Number == int.Parse(entry.Number)
                );
                if (participation == null)
                {
                    continue;
                }
                var isFinal = participation
                    .Phases.Take(participation.Phases.Count - 1)
                    .All(x => x.IsComplete());
                var snapshot = SnapshotFactory.Create(entry, type, isFinal);
                snapshots.Add(snapshot);
            }
            await _judgeRelay.Clients.All.Process(snapshots);
        });
    }
}
