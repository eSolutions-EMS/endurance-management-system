using Core.Domain.Core.Exceptions;

namespace Core.Domain.State.EnduranceEvents;

public class EnduranceEventException : DomainExceptionBase
{
    private static readonly string Name = nameof(EnduranceEvent);

    protected override string Entity => Name;
}
