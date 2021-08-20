using EnduranceJudge.Domain.Aggregates.Import.Competitions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Core.Validation;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.Aggregates.Import.EnduranceEvents
{
    public class EnduranceEvent : DomainBase<EnduranceEventException>, IAggregateRoot
    {
        private EnduranceEvent()
        {
        }

        public EnduranceEvent(List<Competition> competitions): base(default)
        {
            this.competitions = competitions;
        }

        public EnduranceEvent(List<Competition> competitions, string name, string countryIsoCode) : base(default)
            => this.Validate(() =>
            {
                this.competitions = competitions;
                this.Name = name.IsRequired(nameof(name));
                this.CountryIsoCode = countryIsoCode.IsRequired(nameof(countryIsoCode));
            });

        public string Name { get; private set; }
        public string CountryIsoCode { get; private set; }

        private List<Competition> competitions;
        public IReadOnlyList<Competition> Competitions
        {
            get => this.competitions.AsReadOnly();
            private set => this.competitions = value.ToList();
        }
    }
}
