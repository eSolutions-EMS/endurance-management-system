using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.States;
using EnduranceJudge.Gateways.Persistence.Core;
using EnduranceJudge.Gateways.Persistence.Entities.EnduranceEvents;
using EnduranceJudge.Gateways.Persistence.Entities.ParticipantsInCompetitions;
using EnduranceJudge.Gateways.Persistence.Entities.Phases;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EnduranceJudge.Gateways.Persistence.Entities.Competitions
{
    public class CompetitionEntity : EntityBase, ICompetitionState
    {
        private static readonly Type ImportType = typeof(Domain.Aggregates.Import.Competitions.Competition);
        private static readonly Type EventType = typeof(Domain.Aggregates.Event.Competitions.Competition);
        private static readonly Type RankingType = typeof(Domain.Aggregates.Ranking.Competitions.Competition);

        public CompetitionType Type { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public EnduranceEventEntity EnduranceEvent { get; set; }
        public int EnduranceEventId { get; set; }

        [JsonIgnore]
        public IList<PhaseEntity> Phases { get; set; }

        [JsonIgnore]
        public IList<ParticipantInCompetition> ParticipantsInCompetitions { get; set; }

        public override IEnumerable<Type> DomainTypes { get; } = new[] { ImportType, EventType, RankingType };
    }
}
