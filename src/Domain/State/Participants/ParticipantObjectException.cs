using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.State.Participants
{
    public class ParticipantObjectException : DomainObjectException
    {
        protected override string Entity { get; } = nameof(Participant);
    }
}
