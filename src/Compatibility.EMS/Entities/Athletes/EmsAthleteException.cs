using NTS.Compatibility.EMS.Abstractions;

namespace NTS.Compatibility.EMS.Entities.Athletes;

public class EmsAthleteException : EmsDomainExceptionBase
{
    protected override string Entity { get; } = nameof(EmsAthlete);
}
