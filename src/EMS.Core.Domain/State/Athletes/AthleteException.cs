using EMS.Core.Domain.Core.Exceptions;

namespace EMS.Core.Domain.State.Athletes;

public class AthleteException : DomainExceptionBase
{
    protected override string Entity { get; } = nameof(Athlete);
}