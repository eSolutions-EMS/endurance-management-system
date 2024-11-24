using NTS.Compatibility.EMS.Abstractions;

namespace NTS.Compatibility.EMS.Entities.Results;

public class EmsResultException : EmsDomainExceptionBase
{
    protected override string Entity { get; } = nameof(EmsResult);
}
