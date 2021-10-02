using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.Aggregates.State.Participations
{
    public class ParticipationException : DomainException
    {
        protected override string Entity { get; } = nameof(Participation);
    }
}
