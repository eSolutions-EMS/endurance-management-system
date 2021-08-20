using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.Aggregates.Common.Countries
{
    public class CountryException : DomainException
    {
        protected override string Entity { get; } = nameof(Country);
    }
}
