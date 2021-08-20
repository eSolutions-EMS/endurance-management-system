using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.Aggregates.Ranking.ParticipationsInPhases
{
    public class RankingParticipationInPhaseException : DomainException
    {
        protected override string Entity { get; } = $"Ranking {nameof(ParticipationInPhase)}";
    }
}
