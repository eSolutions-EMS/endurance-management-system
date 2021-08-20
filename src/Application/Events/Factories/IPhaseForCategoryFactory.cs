using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Domain.Aggregates.Event.PhasesForCategory;

namespace EnduranceJudge.Application.Events.Factories
{
    public interface IPhaseForCategoryFactory : IService
    {
        PhaseForCategory Create(IPhaseForCategoryState data);
    }
}
