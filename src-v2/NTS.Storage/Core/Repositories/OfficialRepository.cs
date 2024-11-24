using Not.Storage.Repositories;
using NTS.Domain.Core.Aggregates;

namespace NTS.Storage.Core.Repositories;

public class OfficialRepository : SetRepository<Official, CoreState>
{
    public OfficialRepository(IStore<CoreState> store)
        : base(store) { }
}
