using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.States;

namespace EnduranceJudge.Domain.Aggregates.Manager.DTOs
{
    public class PhaseForCategoryDto : IPhaseForCategoryState
    {
        public int Id { get; }
        public int MaxRecoveryTimeInMinutes { get; }
        public int RestTimeInMinutes { get; }
        public Category Category { get; }
    }
}
