using EnduranceJudge.Application.Events.Common;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Aggregates.Event.Phases;
using EnduranceJudge.Domain.Aggregates.Manager.DTOs;
using EnduranceJudge.Domain.States;
using EnduranceJudge.Gateways.Persistence.Core;
using EnduranceJudge.Gateways.Persistence.Entities.Competitions;
using EnduranceJudge.Gateways.Persistence.Entities.ParticipationsInPhases;
using EnduranceJudge.Gateways.Persistence.Entities.PhasesForCategories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EnduranceJudge.Gateways.Persistence.Entities.Phases
{
    public class PhaseEntity : EntityBase, IPhaseState,
        IMap<Phase>,
        IMapTo<PhaseDependantModel>,
        IMapTo<PhaseDto>
    {
        private static readonly Type Domain = typeof(Phase);

        public int LengthInKm { get; set; }
        public bool IsFinal { get; set; }
        public int OrderBy { get; }

        [JsonIgnore]
        public CompetitionEntity Competition { get; set; }
        public int CompetitionId { get; set; }
        [JsonIgnore]
        public IList<PhaseForCategoryEntity> PhasesForCategories { get; set; }
        [JsonIgnore]
        public IList<ParticipationInPhaseEntity> ParticipationsInPhases { get; set; }

        public override IEnumerable<Type> DomainTypes { get; } = new[] { Domain };
    }
}
