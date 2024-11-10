using NTS.Compatibility.EMS.Abstractions;

namespace NTS.Compatibility.EMS.Entities.EnduranceEvents;

public class EmsEnduranceEventException : EmsDomainExceptionBase
{
    static readonly string Name = nameof(EmsEnduranceEvent);

    protected override string Entity => Name;
}
