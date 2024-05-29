using NTS.Compatibility.EMS.Abstractions;

namespace NTS.Compatibility.EMS.Entities.Athletes;

public class AthleteException : DomainExceptionBase
{
    protected override string Entity { get; } = nameof(Athlete);
}
