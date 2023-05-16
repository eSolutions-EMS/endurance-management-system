using EMS.Core.Domain.Core.Exceptions;

namespace EMS.Core.Domain.State.Countries;

public class CountryException : DomainExceptionBase
{
    protected override string Entity { get; } = nameof(Country);
}
