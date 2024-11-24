using NTS.Compatibility.EMS.Abstractions;

namespace NTS.Compatibility.EMS.Entities.Horses;

public class EmsHorseException : EmsDomainExceptionBase
{
    protected override string Entity { get; } = nameof(EmsHorse);
}
