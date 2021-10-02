using EnduranceJudge.Core.Extensions;
using EnduranceJudge.Domain.Aggregates.State.PhasesForCategory;
using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.States;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.Aggregates.State.Phases
{
    public class Phase : DomainObjectBase<PhaseException>, IPhaseState
    {
        public Phase(IPhaseState data) : base(data.Id) => this.Validate(() =>
        {
            this.IsFinal = data.IsFinal;
            this.OrderBy = data.OrderBy.IsRequired(nameof(data.OrderBy));
            this.LengthInKm = data.LengthInKm.IsRequired(nameof(data.LengthInKm));
        });

        private List<PhaseForCategory> phasesForCategories = new();
        public bool IsFinal { get; private set; }
        public int OrderBy { get; private set; }
        public int LengthInKm { get; private set; }

        public void Add(PhaseForCategory phaseForCategory) => this.Validate(() =>
        {
            this.phasesForCategories.AddOrUpdateObject(phaseForCategory);
        });

        public IReadOnlyList<PhaseForCategory> PhasesForCategories
        {
            get => this.phasesForCategories.AsReadOnly();
            private set => this.phasesForCategories = value.ToList();
        }
    }
}
