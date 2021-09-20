using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Aggregates.Manager.ParticipationsInPhases;
using EnduranceJudge.Domain.States;
using EnduranceJudge.Gateways.Persistence.Core;
using EnduranceJudge.Gateways.Persistence.Entities.ParticipantsInCompetitions;
using EnduranceJudge.Gateways.Persistence.Entities.Phases;
using EnduranceJudge.Gateways.Persistence.Entities.ResultsInPhases;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace EnduranceJudge.Gateways.Persistence.Entities.ParticipationsInPhases
{
    public class ParticipationInPhaseEntity : EntityBase, IParticipationInPhaseState
    {
        private static readonly Type ManagerDomain = typeof(ParticipationInPhase);

        public DateTime StartTime { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public DateTime? InspectionTime { get; set; }
        public DateTime? ReInspectionTime { get; set; }

        [JsonIgnore]
        public ParticipantInCompetitionEntity ParticipationInCompetition { get; set; }
        public int ParticipationInCompetitionId { get; set; }
        [JsonIgnore]
        public ResultInPhaseEntity ResultInPhase { get; set; }
        public int? ResultInPhaseId { get; set; }
        [JsonIgnore]
        public PhaseEntity Phase { get; set; }
        public int PhaseId { get; set; }

        public override IEnumerable<Type> DomainTypes { get; } = new[] { ManagerDomain };
    }
}
