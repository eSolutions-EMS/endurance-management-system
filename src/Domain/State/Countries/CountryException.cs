using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.State.Countries
{
    public class CountryException : DomainException
    {
        protected override string Entity { get; } = nameof(Country);
    }
}
