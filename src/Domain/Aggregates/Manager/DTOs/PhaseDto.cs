using EnduranceJudge.Domain.States;
using System.Collections.Generic;

namespace EnduranceJudge.Domain.Aggregates.Manager.DTOs
{
    public class PhaseDto : IPhaseState
    {
        public int Id { get; private set; }
        public int OrderBy { get; private set; }
        public int LengthInKm { get; private set; }
        public bool IsFinal { get; private set; }
        public IList<PhaseForCategoryDto> PhasesForCategories { get; private set; }
    }
}
