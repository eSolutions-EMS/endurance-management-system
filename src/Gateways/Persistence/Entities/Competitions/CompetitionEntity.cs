using EnduranceJudge.Domain.Aggregates.Event.Competitions;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Gateways.Persistence.Core;
using EnduranceJudge.Gateways.Persistence.Entities.EnduranceEvents;
using EnduranceJudge.Gateways.Persistence.Entities.ParticipantsInCompetitions;
using EnduranceJudge.Gateways.Persistence.Entities.Phases;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace EnduranceJudge.Gateways.Persistence.Entities.Competitions
{
    public class CompetitionEntity : EntityBase, ICompetitionState
    {
        public CompetitionType Type { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public EnduranceEventEntity EnduranceEvent { get; set; }
        public int EnduranceEventId { get; set; }

        [JsonIgnore]
        public IList<PhaseEntity> Phases { get; set; }

        [JsonIgnore]
        public IList<ParticipantInCompetition> ParticipantsInCompetitions { get; set; }
    }
}
