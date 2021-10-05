using EnduranceJudge.Domain.Core.Models;

namespace EnduranceJudge.Domain.State.Countries
{
    public class Country : DomainObjectBase<CountryException>, ICountryState
    {
        private Country() : base(default)
        {
        }

        public string IsoCode { get; private set; }

        public string Name { get; private set; }
    }
}
