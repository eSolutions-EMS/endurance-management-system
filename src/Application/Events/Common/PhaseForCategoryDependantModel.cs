using EnduranceJudge.Domain.Aggregates.Event.PhasesForCategory;
using EnduranceJudge.Domain.Enums;

namespace EnduranceJudge.Application.Events.Common
{
    public class PhaseForCategoryDependantModel : IPhaseForCategoryState
    {
        public int Id { get; set; }
        public int MaxRecoveryTimeInMinutes { get; set; }
        public int RestTimeInMinutes { get; set; }
        public Category Category { get; set; }
    }
}
