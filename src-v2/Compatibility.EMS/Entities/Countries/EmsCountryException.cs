using NTS.Compatibility.EMS.Abstractions;

namespace NTS.Compatibility.EMS.Entities.Countries;

public class EmsCountryException : EmsDomainExceptionBase
{
    protected override string Entity { get; } = nameof(EmsCountry);
}
