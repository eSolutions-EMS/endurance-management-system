using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.Aggregates.Rankings.PhasePerformances
{
    public class RankingPhasePerformanceObjectException : DomainObjectException
    {
        protected override string Entity { get; } = $"Ranking {nameof(PhasePerformance)}";
    }
}
