using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.State.Laps;

public class LapException : DomainExceptionBase
{
    protected override string Entity { get; } = nameof(Lap);
}
