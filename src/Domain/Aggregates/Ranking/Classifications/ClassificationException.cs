using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.Aggregates.Ranking.Classifications
{
    public class ClassificationException : DomainException
    {
        protected override string Entity { get; } = nameof(Classification);
    }
}
