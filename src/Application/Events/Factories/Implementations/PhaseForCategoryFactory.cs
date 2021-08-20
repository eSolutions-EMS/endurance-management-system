using EnduranceJudge.Domain.Aggregates.Event.PhasesForCategory;

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
