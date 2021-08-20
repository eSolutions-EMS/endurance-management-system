using EnduranceJudge.Application.Events.Commands.EnduranceEvents;
using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Domain.Aggregates.Event.EnduranceEvents;

namespace EnduranceJudge.Application.Events.Factories
{
    public interface IEnduranceEventFactory : IService
    {
        EnduranceEvent Create(SaveEnduranceEvent data);
    }
}
