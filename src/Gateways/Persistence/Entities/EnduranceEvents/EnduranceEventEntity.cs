using EnduranceJudge.Domain.States;
using EnduranceJudge.Gateways.Persistence.Core;
using EnduranceJudge.Gateways.Persistence.Entities.Competitions;
using EnduranceJudge.Gateways.Persistence.Entities.Countries;
using EnduranceJudge.Gateways.Persistence.Entities.Personnels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EnduranceJudge.Gateways.Persistence.Entities.EnduranceEvents
{
    public class EnduranceEventEntity : AggregateRootEntityBase, IEnduranceEventState
    {
        private static readonly Type ImportAggregate = typeof(Domain.Aggregates.Import.EnduranceEvents.EnduranceEvent);
        private static readonly Type EventAggregate = typeof(Domain.Aggregates.Event.EnduranceEvents.EnduranceEvent);

        public string Name { get; set; }
        public string PopulatedPlace { get; set; }
        public string CountryIsoCode { get; set; }

        [JsonIgnore]
        public IList<CompetitionEntity> Competitions { get; set; }
        [JsonIgnore]
        public IList<PersonnelEntity> Personnel { get; set; }
        [JsonIgnore]
        public CountryEntity Country { get; set; }

        public override IEnumerable<Type> DomainTypes { get; } = new[] { ImportAggregate, EventAggregate };
    }
}
