using NTS.Compatibility.EMS.Abstractions;

namespace NTS.Compatibility.EMS.Entities.Horses;

public class HorseException : DomainExceptionBase
{
    protected override string Entity { get; } = nameof(Horse);
}
