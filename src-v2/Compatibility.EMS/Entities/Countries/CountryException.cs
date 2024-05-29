using NTS.Compatibility.EMS.Abstractions;

namespace NTS.Compatibility.EMS.Entities.Countries;

public class CountryException : DomainExceptionBase
{
    protected override string Entity { get; } = nameof(Country);
}
