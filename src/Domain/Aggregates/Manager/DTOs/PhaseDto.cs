using EnduranceJudge.Domain.Aggregates.Event.Phases;
using EnduranceJudge.Domain.Aggregates.Event.PhasesForCategory;
using EnduranceJudge.Domain.Enums;

namespace EnduranceJudge.Domain.Aggregates.Manager.DTOs
{
    public class PhaseDto : IPhaseState, IPhaseForCategoryState
    {
        public int Id { get; }
        public int OrderBy { get; }
        public int LengthInKm { get; }
        public bool IsFinal { get; }
        public int MaxRecoveryTimeInMinutes { get; }
        public int RestTimeInMinutes { get; }
        public Category Category { get; }
    }
}
