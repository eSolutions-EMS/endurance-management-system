using EnduranceJudge.Domain.Core.Models;

namespace EnduranceJudge.Domain.State.Countries
{
    public interface ICountryState : IDomainModelState
    {
        string IsoCode { get; }

        string Name { get; }
    }
}
