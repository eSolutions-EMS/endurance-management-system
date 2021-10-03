using EnduranceJudge.Core.Extensions;
using EnduranceJudge.Domain.Aggregates.State.PhasesForCategory;
using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.States;
using System.Collections.Generic;
using System.Linq;
using static EnduranceJudge.Localization.DesktopStrings;
namespace EnduranceJudge.Domain.Aggregates.State.Phases
{
    public class Phase : DomainObjectBase<PhaseException>, IPhaseState
    {
        private Phase() {}
        public Phase(int id, bool isFinal, int orderBy, int lengthInKm) : base(id) => this.Validate(() =>
        {
            this.IsFinal = isFinal;
            this.OrderBy = orderBy.IsRequired(ORDER);
            this.LengthInKm = lengthInKm.IsRequired(LENGTH_IN_KM);
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
