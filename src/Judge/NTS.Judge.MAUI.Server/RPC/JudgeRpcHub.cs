using Microsoft.AspNetCore.SignalR;
using NTS.ACL.Entities;
using NTS.ACL.Enums;
using NTS.ACL.Factories;
using NTS.ACL.RPC;
using NTS.ACL.RPC.Procedures;
using NTS.Application.RPC;
using NTS.Domain.Core.Objects.Payloads;
using NTS.Domain.Objects;
using Not.Application.CRUD.Ports;
using NTS.Domain.Core.Aggregates;
using Not.Exceptions;
using NTS.Domain.Enums;
using NTS.ACL.Entities.EMS;

namespace NTS.Judge.MAUI.Server.RPC;

public class JudgeRpcHub : Hub<IJudgeClientProcedures>, IJudgeHubProcedures
{
    readonly IRead<Participation> _participations;
    readonly IHubContext<WitnessRpcHub, IEmsClientProcedures> _witnessRelay;

    public JudgeRpcHub(IRead<Participation> participations, IHubContext<WitnessRpcHub, IEmsClientProcedures> witnessRelay)
    {
        _participations = participations;
        _witnessRelay = witnessRelay;
    }

    public async Task ReceiveSnapshot(Snapshot snapshot)
    {
        var entries = new List<EmsParticipantEntry>();
        var participation = _participations.Read(snapshot.Number).Result;
        GuardHelper.ThrowIfDefault(participation);
        var emsParticipation = ParticipationFactory.CreateEms(participation);
        var entry = new EmsParticipantEntry(emsParticipation);
        entries.Add(entry);
        var eventType = (EmsWitnessEventType)snapshot.Type;
        if (snapshot.Type is SnapshotType.Final or SnapshotType.Automatic)
        {
            eventType = EmsWitnessEventType.Arrival;
        }
        await _witnessRelay.Clients.All.ReceiveWitnessEvent(entries, eventType);
    }

    public async Task SendParticipationEliminated(ParticipationEliminated revoked)
    {
        var emsParticipation = ParticipationFactory.CreateEms(revoked.Participation);
        var entry = new EmsParticipantEntry(emsParticipation);
        await _witnessRelay.Clients.All.ReceiveEntryUpdate(entry, EmsCollectionAction.Remove);
    }

    public async Task SendParticipationRestored(ParticipationRestored restored)
    {
        var emsParticipation = ParticipationFactory.CreateEms(restored.Participation);
        var entry = new EmsParticipantEntry(emsParticipation);
        await _witnessRelay.Clients.All.ReceiveEntryUpdate(entry, EmsCollectionAction.AddOrUpdate);
    }

    public async Task SendStartCreated(PhaseCompleted phaseCompleted)
    {
        var emsParticipation = ParticipationFactory.CreateEms(phaseCompleted.Participation);
        var entry = new EmsStartlistEntry(emsParticipation);
        await _witnessRelay.Clients.All.ReceiveEntry(entry, EmsCollectionAction.AddOrUpdate);
    }
}
