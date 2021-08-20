using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.Aggregates.Manager.Participations
{
    public class ManagerParticipationException : DomainException
    {
        protected override string Entity { get; } = $"Manager {nameof(Participation)}";
    }
}
