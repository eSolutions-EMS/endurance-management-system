using NTS.Compatibility.EMS.Abstractions;

namespace NTS.Compatibility.EMS.Entities.Laps;

public class EmsLapException : EmsDomainExceptionBase
{
    protected override string Entity { get; } = nameof(EmsLap);
}
