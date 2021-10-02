using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Domain.Aggregates.State.PhasesForCategory;
using EnduranceJudge.Domain.States;

namespace EnduranceJudge.Application.Events.Factories
{
    public interface IPhaseForCategoryFactory : IService
    {
        PhaseForCategory Create(IPhaseForCategoryState data);
    }
}
