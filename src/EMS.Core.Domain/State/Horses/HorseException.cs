using EMS.Core.Domain.Core.Exceptions;

namespace EMS.Core.Domain.State.Horses;

public class HorseException : DomainExceptionBase
{
    protected override string Entity { get; } = nameof(Horse);
}
