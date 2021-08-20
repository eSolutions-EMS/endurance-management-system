using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Enums;

namespace EnduranceJudge.Domain.Aggregates.Event.PhasesForCategory
{
    public class PhaseForCategory : DomainBase<PhaseForCategoryException>, IPhaseForCategoryState
    {
        private PhaseForCategory()
        {
        }

        public PhaseForCategory(IPhaseForCategoryState data) : base(data.Id)
            => this.Validate(() =>
            {
                this.MaxRecoveryTimeInMinutes = data.MaxRecoveryTimeInMinutes.IsRequired(
                    nameof(data.MaxRecoveryTimeInMinutes));
                this.RestTimeInMinutes = data.RestTimeInMinutes.IsRequired(nameof(data.RestTimeInMinutes));
                this.Category = data.Category.IsRequired(nameof(data.Category));
            });

        public int MaxRecoveryTimeInMinutes { get; private set; }
        public int RestTimeInMinutes { get; private set; }
        public Category Category { get; private set; }
    }
}
