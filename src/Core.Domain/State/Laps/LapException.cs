using Core.Domain.Common.Exceptions;

namespace Core.Domain.State.Laps;

public class LapException : DomainExceptionBase
{
    protected override string Entity { get; } = nameof(Lap);
}
