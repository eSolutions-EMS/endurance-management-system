using EnduranceJudge.Domain.States;
using EnduranceJudge.Gateways.Persistence.Core;
using EnduranceJudge.Gateways.Persistence.Entities.Athletes;
using EnduranceJudge.Gateways.Persistence.Entities.Horses;
using EnduranceJudge.Gateways.Persistence.Entities.ParticipantsInCompetitions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EnduranceJudge.Gateways.Persistence.Entities.Participants
{
    public class ParticipantEntity : EntityBase, IParticipantState
    {
        private static readonly Type ImportDomain = typeof(Domain.Aggregates.Import.Participants.Participant);
        private static readonly Type EventDomain = typeof(Domain.Aggregates.Event.Participants.Participant);
        private static readonly Type ManagerDomain = typeof(Domain.Aggregates.Manager.Participants.Participant);

        public string RfId { get; set; }
        public int Number { get; set; }
        public int? MaxAverageSpeedInKmPh { get; set; }

        [JsonIgnore]
        public HorseEntity Horse { get; set; }
        public int HorseId { get; set; }

        [JsonIgnore]
        public AthleteEntity Athlete { get; set; }
        public int AthleteId { get; set; }

        [JsonIgnore]
        public ICollection<ParticipantInCompetition> ParticipantsInCompetitions { get; set; }

        public override IEnumerable<Type> DomainTypes { get; } = new[] { ImportDomain, EventDomain, ManagerDomain };
    }
}
