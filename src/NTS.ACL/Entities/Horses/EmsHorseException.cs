using NTS.ACL.Abstractions;

namespace NTS.ACL.Entities.Horses;

public class EmsHorseException : EmsDomainExceptionBase
{
    protected override string Entity { get; } = nameof(EmsHorse);
}
