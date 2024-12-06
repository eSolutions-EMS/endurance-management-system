using NTS.ACL.Abstractions;

namespace NTS.ACL.Entities.Participants;

public class EmsParticipantException : EmsDomainExceptionBase
{
    protected override string Entity { get; } = nameof(EmsParticipant);
}
