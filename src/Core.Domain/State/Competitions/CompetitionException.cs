using Core.Domain.Common.Exceptions;

namespace Core.Domain.State.Competitions;

public class CompetitionException : DomainExceptionBase
{
    protected override string Entity { get; } = nameof(Competition);
}
