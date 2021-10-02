using EnduranceJudge.Application.Events.Commands.EnduranceEvents;
using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Domain.Aggregates.State;

namespace EnduranceJudge.Application.Events.Factories
{
    public interface IEnduranceEventFactory : IService
    {
        EventState Create(SaveEvent data);
    }
}
