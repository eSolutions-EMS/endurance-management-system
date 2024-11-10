using NTS.Domain.Core.Entities;
using NTS.Persistence.States;

namespace NTS.Persistence.Adapters;

public class RankingRepository : SetRepository<Ranking, CoreState>
{
    public RankingRepository(IStore<CoreState> store) : base(store) { }
}
