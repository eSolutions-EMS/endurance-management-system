using System.Transactions;
using Not.Application.RPC.Clients;
using Not.Application.RPC.SignalR;
using Not.Injection;
using NTS.Application.RPC;
using NTS.Domain.Core.Objects.Payloads;

namespace NTS.Judge.RPC;

public class JudgeRpcClient : RpcClient, IJudgeRpcClient
{
    public JudgeRpcClient(IRpcSocket socket)
        : base(socket) { }

    public async Task SendParticipationEliminated(ParticipationEliminated revoked, Action<string> log)
    {
        await InvokeHubProcedure(nameof(IJudgeHubProcedures.SendParticipationEliminated), revoked);
        log($"---------- RPC -----------  {nameof(IJudgeHubProcedures.SendParticipationEliminated)} invoked");
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
