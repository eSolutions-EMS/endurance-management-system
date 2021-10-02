using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.States;
using static EnduranceJudge.Localization.DesktopStrings;

namespace EnduranceJudge.Domain.Aggregates.State.PhasesForCategory
{
    public class PhaseForCategory : DomainObjectBase<PhaseForCategoryException>, IPhaseForCategoryState
    {
        public PhaseForCategory(IPhaseForCategoryState data) : base(data.Id) => this.Validate(() =>
        {
            this.MaxRecoveryTimeInMinutes = data.MaxRecoveryTimeInMinutes.IsRequired(MAX_RECOVERY_TIME_IN_MINS);
            this.RestTimeInMinutes = data.RestTimeInMinutes.IsRequired(REST_TIME_IN_MINUTES);
            this.Category = data.Category.IsRequired(CATEGORY);
        });

        public int MaxRecoveryTimeInMinutes { get; private set; }
        public int RestTimeInMinutes { get; private set; }
        public Category Category { get; private set; }
    }
}
