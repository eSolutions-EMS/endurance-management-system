using NTS.ACL.Abstractions;

namespace NTS.ACL.Entities.Countries;

public class EmsCountryException : EmsDomainExceptionBase
{
    protected override string Entity { get; } = nameof(EmsCountry);
}
