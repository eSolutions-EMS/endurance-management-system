using NTS.Domain.Core.Objects.Payloads;

namespace NTS.Application.RPC;

public interface IJudgeHubProcedures
{
    Task SendStartCreated(PhaseCompleted phaseCompleted);
    Task SendParticipationEliminated(ParticipationEliminated revoked, Action<string> log);
    Task SendParticipationRestored(ParticipationRestored restored);
}
