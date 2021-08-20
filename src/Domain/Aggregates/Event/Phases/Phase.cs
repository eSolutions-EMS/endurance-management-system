using EnduranceJudge.Core.Extensions;
using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.Aggregates.Event.PhasesForCategory;
using EnduranceJudge.Domain.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.Aggregates.Event.Phases
{
    public class Phase : DomainBase<PhaseException>, IPhaseState
    {
        private Phase()
        {
        }

        public Phase(IPhaseState data) : base(data.Id)
            => this.Validate(() =>
            {
                this.IsFinal = data.IsFinal;
                this.LengthInKm = data.LengthInKm.IsRequired(nameof(data.LengthInKm));
            });

        public bool IsFinal { get; private set; }
        public int LengthInKm { get; private set; }


        private List<PhaseForCategory> phasesForCategories = new();
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
