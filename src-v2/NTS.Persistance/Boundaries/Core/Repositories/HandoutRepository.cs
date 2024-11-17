using NTS.Domain.Core.Entities;

namespace NTS.Persistence.Boundaries.Core.Repositories;

public class HandoutRepository : SetRepository<Handout, CoreState>
{
    public HandoutRepository(IStore<CoreState> store)
        : base(store) { }
}
