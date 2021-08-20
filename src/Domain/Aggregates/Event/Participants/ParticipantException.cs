using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.Aggregates.Event.Participants
{
    public class ParticipantException : DomainException
    {
        protected override string Entity { get; } = nameof(Participant);
    }
}
