using EMS.Core.Domain.Core.Exceptions;

namespace EMS.Core.Domain.State.Laps;

public class LapException : DomainExceptionBase
{
    protected override string Entity { get; } = nameof(Lap);
}
