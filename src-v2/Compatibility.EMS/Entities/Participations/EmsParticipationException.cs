using NTS.Compatibility.EMS.Abstractions;

namespace NTS.Compatibility.EMS.Entities.Participations;

public class EmsParticipationException : EmsDomainExceptionBase
{
    protected override string Entity { get; } = nameof(EmsParticipation);
}
