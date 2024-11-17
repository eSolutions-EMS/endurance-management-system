using NTS.Domain.Core.Entities;

namespace NTS.Persistence.Boundaries.Core.Repositories;

public class RankingRepository : SetRepository<Ranking, CoreState>
{
    public RankingRepository(IStore<CoreState> store)
        : base(store) { }
}
