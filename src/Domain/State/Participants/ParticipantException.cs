using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.State.Participants
{
    public class ParticipantException : DomainException
    {
        protected override string Entity { get; } = nameof(Participant);
    }
}
