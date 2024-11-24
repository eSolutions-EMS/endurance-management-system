using NTS.Compatibility.EMS.Abstractions;

namespace NTS.Compatibility.EMS.Entities.Personnels;

public class EmsPersonnelException : EmsDomainExceptionBase
{
    protected override string Entity { get; } = nameof(EmsPersonnel);
}
