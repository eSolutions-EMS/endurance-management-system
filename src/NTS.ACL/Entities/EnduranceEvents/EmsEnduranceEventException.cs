using NTS.ACL.Abstractions;

namespace NTS.ACL.Entities.EnduranceEvents;

public class EmsEnduranceEventException : EmsDomainExceptionBase
{
    static readonly string Name = nameof(EmsEnduranceEvent);

    protected override string Entity => Name;
}
