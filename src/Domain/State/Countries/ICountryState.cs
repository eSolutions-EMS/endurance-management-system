using EnduranceJudge.Core.Models;

namespace EnduranceJudge.Domain.State.Countries
{
    public interface ICountryState : IIdentifiable
    {
        string IsoCode { get; }

        string Name { get; }
    }
}
