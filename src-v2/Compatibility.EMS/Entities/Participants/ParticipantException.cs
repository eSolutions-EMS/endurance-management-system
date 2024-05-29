using NTS.Compatibility.EMS.Abstractions;

namespace NTS.Compatibility.EMS.Entities.Participants;

public class ParticipantException : DomainExceptionBase
{
    protected override string Entity { get; } = nameof(Participant);
}
