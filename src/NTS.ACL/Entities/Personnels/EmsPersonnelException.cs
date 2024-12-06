using NTS.ACL.Abstractions;

namespace NTS.ACL.Entities.Personnels;

public class EmsPersonnelException : EmsDomainExceptionBase
{
    protected override string Entity { get; } = nameof(EmsPersonnel);
}
