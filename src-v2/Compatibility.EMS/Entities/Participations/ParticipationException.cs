using NTS.Compatibility.EMS.Abstractions;

namespace NTS.Compatibility.EMS.Entities.Participations;

public class ParticipationException : DomainExceptionBase
{
    protected override string Entity { get; } = nameof(Participation);
}
