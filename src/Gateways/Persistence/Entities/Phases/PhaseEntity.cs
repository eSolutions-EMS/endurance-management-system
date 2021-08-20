using EnduranceJudge.Application.Events.Common;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Aggregates.Event.Phases;
using EnduranceJudge.Gateways.Persistence.Core;
using EnduranceJudge.Gateways.Persistence.Entities.Competitions;
using EnduranceJudge.Gateways.Persistence.Entities.PhasesForCategories;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace EnduranceJudge.Gateways.Persistence.Entities.Phases
{
    public class PhaseEntity : EntityBase, IPhaseState, IMap<Phase>, IMapTo<PhaseDependantModel>
    {
        public int LengthInKm { get; set; }

        public bool IsFinal { get; set; }

        [JsonIgnore]
        public IList<PhaseForCategoryEntity> PhasesForCategories { get; set; }

        public int CompetitionId { get; set; }

        [JsonIgnore]
        public CompetitionEntity Competition { get; set; }
    }
}
