using Not.Storage.Repositories;
using NTS.Domain.Core.Aggregates;

namespace NTS.Storage.Core.Repositories;

public class EnduranceEventRepository : RootRepository<EnduranceEvent, CoreState>
{
    public EnduranceEventRepository(IStore<CoreState> store)
        : base(store) { }
}
