﻿using EnduranceJudge.Domain.States;
using EnduranceJudge.Gateways.Persistence.Core;
using EnduranceJudge.Gateways.Persistence.Entities.ParticipantsInCompetitions;
using EnduranceJudge.Gateways.Persistence.Entities.Phases;
using EnduranceJudge.Gateways.Persistence.Entities.ResultsInPhases;
using System;
using Newtonsoft.Json;

namespace EnduranceJudge.Gateways.Persistence.Entities.ParticipationsInPhases
{
    public class ParticipationInPhaseEntity : EntityBase, IParticipationInPhaseState
    {
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
    }
}
