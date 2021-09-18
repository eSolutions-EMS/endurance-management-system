using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.States;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.Aggregates.Manager.DTOs
{
    public class PhaseDto : IPhaseState
    {
        public int Id { get; }
        public int OrderBy { get; }
        public int LengthInKm { get; }
        public bool IsFinal { get; }
        public IList<PhaseForCategoryDto> PhasesForCategories { get; }

        public PhaseForCategoryDto this[Category category]
        {
            get => this.PhasesForCategories.FirstOrDefault(x => x.Category == category);
        }
    }
}
