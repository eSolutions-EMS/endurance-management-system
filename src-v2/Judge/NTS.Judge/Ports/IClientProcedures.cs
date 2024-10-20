using Not.Injection;
using NTS.Domain.Core.Events.Participations;

namespace NTS.Judge.Ports;

public interface IClientProcedures : ITransientService
{
    Task SendStartCreated(PhaseCompleted phaseCompleted);
    Task SendParticipationEliminated(ParticipationEliminated revoked);
    Task SendParticipationRestored(ParticipationRestored restored);
}
