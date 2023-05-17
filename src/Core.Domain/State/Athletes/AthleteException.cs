using Core.Domain.Common.Exceptions;

namespace Core.Domain.State.Athletes;

public class AthleteException : DomainExceptionBase
{
    protected override string Entity { get; } = nameof(Athlete);
}
