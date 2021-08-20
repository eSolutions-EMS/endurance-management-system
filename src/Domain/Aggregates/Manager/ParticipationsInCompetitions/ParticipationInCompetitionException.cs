using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.Aggregates.Manager.ParticipationsInCompetitions
{
    public class ParticipationInCompetitionException : DomainException
    {
        protected override string Entity { get; } = $"Manager {nameof(ParticipationInCompetition)}";
    }
}
