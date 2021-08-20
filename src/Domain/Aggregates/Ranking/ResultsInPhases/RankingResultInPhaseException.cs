using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.Aggregates.Ranking.ResultsInPhases
{
    public class RankingResultInPhaseException : DomainException
    {
        protected override string Entity { get; } = $"Ranking: {nameof(ResultInPhase)}";
    }
}
