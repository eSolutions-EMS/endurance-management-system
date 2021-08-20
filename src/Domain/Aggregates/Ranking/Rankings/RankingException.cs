using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.Aggregates.Ranking.Rankings
{
    public class RankingException : DomainException
    {
        protected override string Entity { get; } = nameof(Ranking);
    }
}
