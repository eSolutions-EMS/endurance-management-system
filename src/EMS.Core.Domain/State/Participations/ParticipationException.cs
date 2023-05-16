using EMS.Core.Domain.Core.Exceptions;

namespace EMS.Core.Domain.State.Participations;

public class ParticipationException : DomainExceptionBase
{
    protected override string Entity { get; } = nameof(Participation);
}
