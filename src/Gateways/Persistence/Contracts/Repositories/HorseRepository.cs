using EnduranceJudge.Application.Contracts.Horses;
using EnduranceJudge.Domain.Aggregates.Common.Horses;
using EnduranceJudge.Gateways.Persistence.Contracts.WorkFile;
using EnduranceJudge.Gateways.Persistence.Core;
using EnduranceJudge.Gateways.Persistence.DataStores;
using EnduranceJudge.Gateways.Persistence.Entities.Horses;

namespace EnduranceJudge.Gateways.Persistence.Contracts.Repositories
{
    public class HorseRepository : RepositoryBase<IHorseDataStore, HorseEntity, Horse>,
        IHorseQueries
    {
        public HorseRepository(IHorseDataStore dataStore, IWorkFileUpdater workFileUpdater)
            : base(dataStore, workFileUpdater)
        {
        }
    }
}
