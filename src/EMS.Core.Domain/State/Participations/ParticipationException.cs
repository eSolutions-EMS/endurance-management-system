using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.State.Participations;

public class ParticipationException : DomainExceptionBase
{
    protected override string Entity { get; } = nameof(Participation);
}