using Core.Domain.Core.Exceptions;

namespace Core.Domain.State.Competitions;

public class CompetitionException : DomainExceptionBase
{
    protected override string Entity { get; } = nameof(Competition);
}
