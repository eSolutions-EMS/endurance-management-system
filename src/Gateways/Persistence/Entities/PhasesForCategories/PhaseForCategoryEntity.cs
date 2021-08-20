using EnduranceJudge.Application.Events.Common;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Aggregates.Event.PhasesForCategory;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Gateways.Persistence.Core;
using EnduranceJudge.Gateways.Persistence.Entities.Phases;
using Newtonsoft.Json;

namespace EnduranceJudge.Gateways.Persistence.Entities.PhasesForCategories
{
    public class PhaseForCategoryEntity : EntityBase,
        IPhaseForCategoryState,
        IMap<PhaseForCategory>,
        IMapTo<PhaseForCategoryDependantModel>
    {
        public int MaxRecoveryTimeInMinutes { get; set; }

        public int RestTimeInMinutes { get; set; }

        public Category Category { get; set; }

        public int PhaseId { get; set; }

        [JsonIgnore]
        public PhaseEntity Phase { get; set; }
    }
}
