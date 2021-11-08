using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Core.Validation;

namespace EnduranceJudge.Domain.State.Countries
{
    public class Country : DomainObjectBase<CountryException>, ICountryState
    {
        private Country() {}
        internal Country(string isoCode, string name) : base(GENERATE_ID)
        {
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
