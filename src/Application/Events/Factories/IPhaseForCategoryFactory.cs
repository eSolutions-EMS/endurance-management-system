using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Domain.Aggregates.Event.PhasesForCategory;
using EnduranceJudge.Domain.States;

namespace EnduranceJudge.Application.Events.Factories
{
    public interface IPhaseForCategoryFactory : IService
    {
        PhaseForCategory Create(IPhaseForCategoryState data);
    }
}
