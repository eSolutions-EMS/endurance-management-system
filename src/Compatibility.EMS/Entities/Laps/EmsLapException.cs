using NTS.ACL.Abstractions;

namespace NTS.ACL.Entities.Laps;

public class EmsLapException : EmsDomainExceptionBase
{
    protected override string Entity { get; } = nameof(EmsLap);
}
