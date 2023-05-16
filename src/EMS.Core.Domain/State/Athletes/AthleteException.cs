using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.State.Athletes;

public class AthleteException : DomainExceptionBase
{
    protected override string Entity { get; } = nameof(Athlete);
}