using Core.Domain.Common.Exceptions;

namespace Core.Domain.State.Countries;

public class CountryException : DomainExceptionBase
{
    protected override string Entity { get; } = nameof(Country);
}
