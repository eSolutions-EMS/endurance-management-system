using EnduranceJudge.Application.Import.Contracts;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Aggregates.Common.Horses;
using EnduranceJudge.Gateways.Persistence.Contracts.WorkFile;
using EnduranceJudge.Gateways.Persistence.Core;
using EnduranceJudge.Gateways.Persistence.Entities.Horses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Gateways.Persistence.Contracts.Repositories
{
    public class HorseRepository : Repository<HorseEntity, Horse>, IHorseCommands
    {
        public HorseRepository(EnduranceJudgeDbContext dbContext, IWorkFileUpdater workFileUpdater)
            : base(dbContext, workFileUpdater)
        {
        }

        public async Task Create(IEnumerable<Horse> domainModels, CancellationToken token)
        {
            var entities = domainModels.MapEnumerable<HorseEntity>();

            await this.DbContext.Horses.AddRangeAsync(entities, token);
            await this.Persist(token);
        }
    }
}
