using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.States;

namespace EnduranceJudge.Domain.Aggregates.Manager.DTOs
{
    public class PhaseForCategoryDto : IPhaseForCategoryState
    {
        public int Id { get; private set; }
        public int MaxRecoveryTimeInMinutes { get; private set; }
        public int RestTimeInMinutes { get; private set; }
        public Category Category { get; private set; }
    }
}
