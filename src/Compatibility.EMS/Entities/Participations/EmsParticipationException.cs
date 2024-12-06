using NTS.ACL.Abstractions;

namespace NTS.ACL.Entities.Participations;

public class EmsParticipationException : EmsDomainExceptionBase
{
    protected override string Entity { get; } = nameof(EmsParticipation);
}
