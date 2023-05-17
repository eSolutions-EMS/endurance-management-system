using Core.Domain.Core.Exceptions;

namespace Core.Domain.State.Participations;

public class ParticipationException : DomainExceptionBase
{
    protected override string Entity { get; } = nameof(Participation);
}
