using NTS.Compatibility.EMS.Abstractions;

namespace NTS.Compatibility.EMS.Entities.Personnels;

public class PersonnelException : DomainExceptionBase
{
    protected override string Entity { get; } = nameof(Personnel);
}
