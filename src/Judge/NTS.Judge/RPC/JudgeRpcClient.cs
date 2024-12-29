using System.Transactions;
using Not.Application.RPC.Clients;
using Not.Application.RPC.SignalR;
using Not.Injection;
using NTS.Application.RPC;
using NTS.Domain.Core.Objects.Payloads;
using NTS.Domain.Objects;

namespace NTS.Judge.RPC;

public class JudgeRpcClient : RpcClient, IJudgeRpcClient
{
    public JudgeRpcClient(IRpcSocket socket)
        : base(socket) { }

    public async Task ReceiveSnapshots(IEnumerable<Snapshot> snapshots)
    {
        await InvokeHubProcedure(nameof(IJudgeHubProcedures.ReceiveSnapshots), snapshots);
    }

    public async Task SendParticipationEliminated(ParticipationEliminated revoked)
    {
        await InvokeHubProcedure(nameof(IJudgeHubProcedures.SendParticipationEliminated), revoked);
    }

    public async Task SendParticipationRestored(ParticipationRestored restored)
    {
        await InvokeHubProcedure(nameof(IJudgeHubProcedures), restored);
    }

    public async Task SendStartCreated(PhaseCompleted phaseCompleted)
    {
        await InvokeHubProcedure(nameof(IJudgeHubProcedures), phaseCompleted);
    }
}

public interface IJudgeRpcClient : IJudgeHubProcedures, IRpcClient, ITransient { }
