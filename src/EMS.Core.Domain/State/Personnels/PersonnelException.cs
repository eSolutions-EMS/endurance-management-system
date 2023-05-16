using EMS.Core.Domain.Core.Exceptions;

namespace EMS.Core.Domain.State.Personnels;

public class PersonnelException : DomainExceptionBase
{
    protected override string Entity { get; } = nameof(Personnel);
}
