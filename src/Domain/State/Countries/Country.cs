using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Core.Validation;

namespace EnduranceJudge.Domain.State.Countries
{
    public class Country : DomainObjectBase<CountryException>, ICountryState
    {
        private Country() {}
        public Country(string isoCode, string name, int id) : base(GENERATE_ID)
        {
            this.Id = id;
            this.Validate(() =>
            {
                this.IsoCode = isoCode.IsRequired(nameof(isoCode));
                this.Name = name.IsRequired(nameof(name));
            });
        }

        public string IsoCode { get; private set; }
        public string Name { get; private set; }
    }
}
