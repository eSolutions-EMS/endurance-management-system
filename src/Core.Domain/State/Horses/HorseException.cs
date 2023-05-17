using Core.Domain.Core.Exceptions;

namespace Core.Domain.State.Horses;

public class HorseException : DomainExceptionBase
{
    protected override string Entity { get; } = nameof(Horse);
}
