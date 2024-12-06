using NTS.ACL.Abstractions;

namespace NTS.ACL.Entities.Results;

public class EmsResultException : EmsDomainExceptionBase
{
    protected override string Entity { get; } = nameof(EmsResult);
}
