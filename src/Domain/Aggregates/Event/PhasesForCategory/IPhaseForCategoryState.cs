using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Enums;

namespace EnduranceJudge.Domain.Aggregates.Event.PhasesForCategory
{
    public interface IPhaseForCategoryState : IDomainModelState
    {
        int MaxRecoveryTimeInMinutes { get; }
        int RestTimeInMinutes { get; }
        Category Category { get; }
    }
}
