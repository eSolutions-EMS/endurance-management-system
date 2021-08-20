using EnduranceJudge.Application.Events.Queries.GetCountriesList;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Core.Models;
using EnduranceJudge.Domain.Aggregates.Common.Countries;
using EnduranceJudge.Gateways.Persistence.Entities.Athletes;
using EnduranceJudge.Gateways.Persistence.Entities.EnduranceEvents;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace EnduranceJudge.Gateways.Persistence.Entities.Countries
{
    public class CountryEntity : ObjectBase, ICountryState,
        IMap<Country>,
        IMapTo<CountryListModel>
    {
        public string IsoCode { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public IList<AthleteEntity> Athletes { get; set; }

        [JsonIgnore]
        public IList<EnduranceEventEntity> EnduranceEvents { get; set; }
    }
}
