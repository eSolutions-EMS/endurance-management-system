using EnduranceJudge.Gateways.Persistence.Core;
using EnduranceJudge.Gateways.Persistence.Entities.Athletes;
using Microsoft.EntityFrameworkCore;

namespace EnduranceJudge.Gateways.Persistence.DataStores
{
    public interface IAthleteDataStore : IDataStore
    {
        public DbSet<AthleteEntity> Athletes { get; set; }
    }
}
