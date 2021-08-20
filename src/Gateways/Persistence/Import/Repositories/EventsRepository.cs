using EnduranceJudge.Application.Import.Contracts;
using EnduranceJudge.Domain.Aggregates.Import.EnduranceEvents;
using EnduranceJudge.Gateways.Persistence.Contracts.Repositories.Events;
using EnduranceJudge.Gateways.Persistence.Contracts.WorkFile;
using EnduranceJudge.Gateways.Persistence.Core;
using EnduranceJudge.Gateways.Persistence.Entities.EnduranceEvents;

namespace EnduranceJudge.Gateways.Persistence.Import.Repositories
{
    internal class EnduranceEventsRepository
        : RepositoryBase<IEnduranceEventsDataStore, EnduranceEventEntity, EnduranceEvent>,
        IEnduranceEventCommands
    {
        public EnduranceEventsRepository(IEnduranceEventsDataStore dataStore, IWorkFileUpdater workFileUpdater)
            : base(dataStore, workFileUpdater)
        {
        }
    }
}
