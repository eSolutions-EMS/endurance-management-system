using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.State.Countries
{
    public class CountryException : DomainExceptionBase
    {
        protected override string Entity { get; } = nameof(Country);
    }
}
