using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.Aggregates.Ranking.Competitions
{
    public class RankingCompetitionException : DomainException
    {
        protected override string Entity { get; } = $"Ranking {nameof(Competition)}";
    }
}
