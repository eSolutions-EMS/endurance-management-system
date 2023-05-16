using EMS.Core.Domain.Core.Exceptions;

namespace EMS.Core.Domain.State.Participants;

public class ParticipantException : DomainExceptionBase
{
    protected override string Entity { get; } = nameof(Participant);
}
