using Not.Storage.Repositories.Adapters;
using NTS.Domain.Core.Entities;

namespace NTS.Storage.Boundaries.Core.Repositories;

public class EnduranceEventRepository : RootRepository<EnduranceEvent, CoreState>
{
    public EnduranceEventRepository(IStore<CoreState> store)
        : base(store) { }
}
