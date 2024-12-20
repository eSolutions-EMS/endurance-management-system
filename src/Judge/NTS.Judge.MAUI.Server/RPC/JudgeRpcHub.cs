using Microsoft.AspNetCore.SignalR;
using NTS.ACL.Entities;
using NTS.ACL.Enums;
using NTS.ACL.Factories;
using NTS.ACL.RPC;
using NTS.ACL.RPC.Procedures;
using NTS.Application.RPC;
using NTS.Domain.Core.Objects.Payloads;
using NTS.Domain.Core.Objects.Startlists;
using NTS.Judge.MAUI.Server.RPC.Procedures;

namespace NTS.Judge.MAUI.Server.RPC;

public class JudgeRpcHub : Hub<IJudgeClientProcedures>, IJudgeHubProcedures
{
    readonly IHubContext<WitnessRpcHub, IClientProcedures> _witnessRelay;

    public JudgeRpcHub(IHubContext<WitnessRpcHub, IClientProcedures> witnessRelay)
    {
        _witnessRelay = witnessRelay;
    }

    public async Task SendParticipationEliminated(ParticipationEliminated revoked)
    {
        await _witnessRelay.Clients.All.ReceiveEntryUpdate(revoked.Participation);
    }

    public async Task SendParticipationRestored(ParticipationRestored restored)
    {
        await _witnessRelay.Clients.All.ReceiveEntryUpdate(restored.Participation);
    }

    public async Task SendStartCreated(PhaseCompleted phaseCompleted)
    {
        var startlistEntry = new StartlistEntry(phaseCompleted.Participation);
        await _witnessRelay.Clients.All.ReceiveEntry(startlistEntry);
    }
}
