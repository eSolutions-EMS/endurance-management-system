using EnduranceJudge.Application.Events.Common;
using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Domain.Aggregates.State.Phases;

namespace EnduranceJudge.Application.Events.Factories
{
    public interface IPhaseFactory : IService
    {
        public Phase Create(PhaseDependantModel data);
    }
}
