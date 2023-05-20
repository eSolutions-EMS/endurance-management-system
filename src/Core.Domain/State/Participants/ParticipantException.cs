using Core.Domain.Common.Exceptions;

namespace Core.Domain.State.Participants;

public class ParticipantException : DomainExceptionBase
{
    protected override string Entity { get; } = nameof(Participant);
}
