using EnduranceJudge.Domain.Aggregates.State.PhasesForCategory;
using EnduranceJudge.Domain.States;

namespace EnduranceJudge.Application.Events.Factories.Implementations
{
    public class PhaseForCategoryFactory : IPhaseForCategoryFactory
    {
        public PhaseForCategory Create(IPhaseForCategoryState data)
        {
            var phaseForCategory = new PhaseForCategory(data);
            return phaseForCategory;
        }
    }
}
