using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.States;
using static EnduranceJudge.Localization.DesktopStrings;

namespace EnduranceJudge.Domain.State.PhasesForCategory
{
    public class PhaseForCategory : DomainObjectBase<PhaseForCategoryException>, IPhaseForCategoryState
    {
        private PhaseForCategory() {}
        public PhaseForCategory(int id, int maxRecoveryTime, int restTime, Category  category) : base(id)
            => this.Validate(() =>
        {
            this.MaxRecoveryTimeInMinutes = maxRecoveryTime.IsRequired(MAX_RECOVERY_TIME_IN_MINS);
            this.RestTimeInMinutes = restTime.IsRequired(REST_TIME_IN_MINUTES);
            this.Category = category.IsRequired(CATEGORY);
        });

        public int MaxRecoveryTimeInMinutes { get; private set; }
        public int RestTimeInMinutes { get; private set; }
        public Category Category { get; private set; }
    }
}
