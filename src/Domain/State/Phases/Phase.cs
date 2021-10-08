using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.Core.Models;
using static EnduranceJudge.Localization.DesktopStrings;
namespace EnduranceJudge.Domain.State.Phases
{
    public class Phase : DomainObjectBase<PhaseException>, IPhaseState
    {
        private Phase() {}
        public Phase(bool isFinal, int orderBy, int lengthInKm, int maxRecovery, int restTime) : base(true)
            => this.Validate(() =>
        {
            this.IsFinal = isFinal;
            this.OrderBy = orderBy.IsRequired(ORDER);
            this.LengthInKm = lengthInKm.IsRequired(LENGTH_IN_KM);
            this.MaxRecoveryTimeInMins = maxRecovery.IsRequired(RECOVERY_IN_MINUTES_TEXT);
            this.RestTimeInMins = restTime.IsRequired(REST_TIME_IN_MINS);
        });
        public bool IsFinal { get; private set; }
        public int OrderBy { get; private set; }
        public int LengthInKm { get; private set; }
        public int MaxRecoveryTimeInMins { get; private set; }
        public int RestTimeInMins { get; private set; }
    }
}
