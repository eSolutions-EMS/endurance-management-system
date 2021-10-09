using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.State.Countries
{
    public class CountryObjectException : DomainObjectException
    {
        protected override string Entity { get; } = nameof(Country);
    }
}
