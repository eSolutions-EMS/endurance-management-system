using NTS.Domain.Core.Entities;
using NTS.Storage.Boundaries.Core;

namespace NTS.Storage.Boundaries.Core.Repositories;

public class OfficialRepository : SetRepository<Official, CoreState>
{
    public OfficialRepository(IStore<CoreState> store)
        : base(store) { }
}
