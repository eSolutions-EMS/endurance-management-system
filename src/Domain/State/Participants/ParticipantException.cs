using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.State.Participants;

public class ParticipantException : DomainExceptionBase
{
    protected override string Entity { get; } = nameof(Participant);
}