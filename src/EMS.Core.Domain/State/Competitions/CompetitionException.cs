using EMS.Core.Domain.Core.Exceptions;

namespace EMS.Core.Domain.State.Competitions;

public class CompetitionException : DomainExceptionBase
{
    protected override string Entity { get; } = nameof(Competition);
}
