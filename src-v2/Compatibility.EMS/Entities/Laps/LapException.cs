using NTS.Compatibility.EMS.Abstractions;

namespace NTS.Compatibility.EMS.Entities.Laps;

public class LapException : DomainExceptionBase
{
    protected override string Entity { get; } = nameof(Lap);
}
