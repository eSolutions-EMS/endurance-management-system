using System.Transactions;
using Not.Injection;
using NTS.Application.RPC;
using NTS.Application.RPC.Procedures;
using NTS.Domain.Core.Objects.Payloads;

namespace NTS.Judge.RPC;

public class JudgeRpcClient : RpcClient, IJudgeRpcClient
{
    public JudgeRpcClient(IRpcSocket socket) : base(socket)
    {
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

public interface IJudgeRpcClient : IJudgeHubProcedures, IRpcClient, ITransient
{
}
