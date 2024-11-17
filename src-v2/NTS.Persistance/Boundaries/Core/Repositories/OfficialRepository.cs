using NTS.Domain.Core.Entities;

namespace NTS.Persistence.Boundaries.Core.Repositories;

public class OfficialRepository : SetRepository<Official, CoreState>
{
    public OfficialRepository(IStore<CoreState> store)
        : base(store) { }
}
