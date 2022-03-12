using EnduranceJudge.Domain.Core.Models;

namespace EnduranceJudge.Domain.State.Phases
{
    public class Phase : DomainBase<PhaseException>, IPhaseState
    {
        private Phase() {}
        public Phase(IPhaseState state) : base(GENERATE_ID)
        {
            this.IsFinal = state.IsFinal;
            this.OrderBy = state.OrderBy;
            this.LengthInKm = state.LengthInKm;
            this.MaxRecoveryTimeInMins = state.MaxRecoveryTimeInMins;
            this.RestTimeInMins = state.RestTimeInMins;
            this.IsCompulsoryInspectionRequired = state.IsCompulsoryInspectionRequired;
        }

        public bool IsFinal { get; internal set; }
        public int OrderBy { get; internal set; }
        public double LengthInKm { get; internal set; }
        public int MaxRecoveryTimeInMins { get; internal set; }
        public int RestTimeInMins { get; internal set; }
        public bool IsCompulsoryInspectionRequired { get; internal set; }
    }
}
