using EnduranceJudge.Application.Contracts.Events;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Aggregates.Event.EnduranceEvents;
using EnduranceJudge.Gateways.Persistence.Contracts.WorkFile;
using EnduranceJudge.Gateways.Persistence.Core;
using EnduranceJudge.Gateways.Persistence.Entities.EnduranceEvents;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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

        public override async Task<TModel> Find<TModel>(int id)
        {
            var entity = await this.DataStore
                .EnduranceEvents
                .Include(e => e.Country)
                .FirstOrDefaultAsync(x => x.Id == id);

            var result = entity.Map<TModel>();

            return result;
        }
    }
}
