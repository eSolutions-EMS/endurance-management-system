using EnduranceJudge.Domain.Aggregates.Common.Athletes;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.States;
using EnduranceJudge.Gateways.Persistence.Core;
using EnduranceJudge.Gateways.Persistence.Entities.Countries;
using EnduranceJudge.Gateways.Persistence.Entities.Participants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EnduranceJudge.Gateways.Persistence.Entities.Athletes
{
    public class AthleteEntity : AggregateRootEntityBase, IAthleteState
    {
        private static readonly Type AthleteType = typeof(Athlete);

        public string FeiId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public Category Category { get; set; }

        [JsonIgnore]
        public ParticipantEntity Participant { get; set; }

        [JsonIgnore]
        public CountryEntity Country { get; set; }
        public string CountryIsoCode { get; set; }

        public override IEnumerable<Type> DomainTypes { get; } = new[] { AthleteType };
    }
}
