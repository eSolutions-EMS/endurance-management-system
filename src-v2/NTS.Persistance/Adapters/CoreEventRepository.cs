using Not.Storage.Repositories.Adapters;
using NTS.Domain.Core.Entities;
using NTS.Persistence.States;

namespace NTS.Persistence.Adapters;

public class CoreEventRepository : RootRepository<EnduranceEvent, CoreState>
{
    public CoreEventRepository(IStore<CoreState> store)
        : base(store) { }
}
