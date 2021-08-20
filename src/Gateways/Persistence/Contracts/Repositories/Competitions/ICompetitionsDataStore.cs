using EnduranceJudge.Gateways.Persistence.Core;
using EnduranceJudge.Gateways.Persistence.Entities.Competitions;
using Microsoft.EntityFrameworkCore;

namespace EnduranceJudge.Gateways.Persistence.Contracts.Repositories.Competitions
{
    public interface ICompetitionsDataStore : IDataStore
    {
        public DbSet<CompetitionEntity> Competitions { get; set; }
    }
}
