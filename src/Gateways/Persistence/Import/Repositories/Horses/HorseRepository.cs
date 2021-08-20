using EnduranceJudge.Application.Import.Contracts;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Aggregates.Common.Horses;
using EnduranceJudge.Gateways.Persistence.Contracts.WorkFile;
using EnduranceJudge.Gateways.Persistence.Core;
using EnduranceJudge.Gateways.Persistence.DataStores;
using EnduranceJudge.Gateways.Persistence.Entities.Horses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Gateways.Persistence.Import.Repositories.Horses
{
    public class HorseRepository : RepositoryBase<IHorseDataStore, HorseEntity, Horse>, IHorseCommands
    {
        public HorseRepository(IHorseDataStore dataStore, IWorkFileUpdater workFileUpdater)
            : base(dataStore, workFileUpdater)
        {
        }

        public async Task Create(IEnumerable<Horse> domainModels, CancellationToken cancellationToken)
        {
            var entities = domainModels.MapEnumerable<HorseEntity>();

            this.DataStore.AddRange(entities);
            await this.DataStore.SaveChangesAsync(cancellationToken);
        }
    }
}
