using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.Aggregates.Event.Competitions
{
    public class CompetitionException : DomainException
    {
        protected override string Entity { get; } = nameof(Competition);
    }
}
