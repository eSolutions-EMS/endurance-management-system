using EnduranceJudge.Domain.Core.Models;

namespace EnduranceJudge.Domain.State.Phases
{
    public class Phase : DomainObjectBase<PhaseException>, IPhaseState
    {
        private Phase() {}
        public Phase(IPhaseState state) : base(GENERATE_ID)
        {
            this.IsFinal = state.IsFinal;
            this.OrderBy = state.OrderBy;
            this.LengthInKm = state.LengthInKm;
            this.MaxRecoveryTimeInMins = state.MaxRecoveryTimeInMins;
            this.RestTimeInMins = state.RestTimeInMins;
        }

        public bool IsFinal { get; internal set; }
        public int OrderBy { get; internal set; }
        public int LengthInKm { get; internal set; }
        public int MaxRecoveryTimeInMins { get; internal set; }
        public int RestTimeInMins { get; internal set; }
    }
}
