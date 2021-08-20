using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.Aggregates.Manager.Participants
{
    public class ManagerParticipantException : DomainException
    {
        protected override string Entity { get; } = $"Manager {nameof(Participant)}";
    }
}
