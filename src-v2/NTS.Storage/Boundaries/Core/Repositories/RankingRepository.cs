using NTS.Domain.Core.Entities;
using NTS.Storage.Boundaries.Core;

namespace NTS.Storage.Boundaries.Core.Repositories;

public class RankingRepository : SetRepository<Ranking, CoreState>
{
    public RankingRepository(IStore<CoreState> store)
        : base(store) { }
}
