using EnduranceJudge.Application.Contracts.Events;
using EnduranceJudge.Domain.Aggregates.Event.EnduranceEvents;
using EnduranceJudge.Gateways.Persistence.Contracts.WorkFile;
using EnduranceJudge.Gateways.Persistence.Core;
using EnduranceJudge.Gateways.Persistence.Entities.EnduranceEvents;

namespace EnduranceJudge.Gateways.Persistence.Contracts.Repositories.Events
{
    internal class EnduranceEventsRepository
        : RepositoryBase<IEnduranceEventsDataStore, EnduranceEventEntity, EnduranceEvent>,
        IEnduranceEventCommands
    {
        public EnduranceEventsRepository(
            IEnduranceEventsDataStore dataStore,
            IWorkFileUpdater workFileUpdater) : base(dataStore, workFileUpdater)
        {
        }
    }
}
