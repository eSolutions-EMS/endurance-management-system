using NTS.Compatibility.EMS.Abstractions;

namespace NTS.Compatibility.EMS.Entities.Participants;

public class EmsParticipantException : EmsDomainExceptionBase
{
    protected override string Entity { get; } = nameof(EmsParticipant);
}
