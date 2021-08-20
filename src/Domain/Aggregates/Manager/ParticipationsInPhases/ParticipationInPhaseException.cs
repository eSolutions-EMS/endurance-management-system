using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.Aggregates.Manager.ParticipationsInPhases
{
    public class ParticipationInPhaseException : DomainException
    {
        protected override string Entity { get; } = nameof(ParticipationInPhase);
    }
}
