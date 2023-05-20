using Core.Domain.Common.Exceptions;

namespace Core.Domain.State.Personnels;

public class PersonnelException : DomainExceptionBase
{
    protected override string Entity { get; } = nameof(Personnel);
}
