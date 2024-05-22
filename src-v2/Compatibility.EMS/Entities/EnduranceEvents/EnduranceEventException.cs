using NTS.Compatibility.EMS.Abstractions;

namespace NTS.Compatibility.EMS.Entities.EnduranceEvents;

public class EnduranceEventException : DomainExceptionBase
{
    private static readonly string Name = nameof(EnduranceEvent);

    protected override string Entity => Name;
}
