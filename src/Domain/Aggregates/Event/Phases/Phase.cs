using EnduranceJudge.Core.Extensions;
using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.Aggregates.Event.PhasesForCategory;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.States;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.Aggregates.Event.Phases
{
    public class Phase : DomainBase<PhaseException>, IPhaseState
    {
        private List<PhaseForCategory> phasesForCategories = new();

        private Phase()
        {
        }

        public Phase(IPhaseState data) : base(data.Id)
            => this.Validate(() =>
            {
                this.IsFinal = data.IsFinal;
                this.OrderBy = data.OrderBy.IsRequired(nameof(data.OrderBy));
                this.LengthInKm = data.LengthInKm.IsRequired(nameof(data.LengthInKm));
            });

        public bool IsFinal { get; private set; }
        public int OrderBy { get; private set; }
        public int LengthInKm { get; private set; }

        public IReadOnlyList<PhaseForCategory> PhasesForCategories
        {
            get => this.phasesForCategories.AsReadOnly();
            private set => this.phasesForCategories = value.ToList();
        }
        public Phase Add(PhaseForCategory phaseForCategory)
        {
            this.phasesForCategories.AddOrUpdateObject(phaseForCategory);
            return this;
        }
    }
}
