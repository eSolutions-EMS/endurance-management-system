using Not.Injection;
using NTS.Domain.Core.Objects.Payloads;

namespace NTS.Application.RPC.Procedures;

public interface IJudgeHubProcedures : ITransient
{
    Task SendStartCreated(PhaseCompleted phaseCompleted);
    Task SendParticipationEliminated(ParticipationEliminated revoked);
    Task SendParticipationRestored(ParticipationRestored restored);
}
