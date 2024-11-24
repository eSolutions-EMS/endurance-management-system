using Not.Storage.Repositories.Adapters;
using NTS.Domain.Setup.Aggregates;

namespace NTS.Storage.Setup.Repositories;

public class EnduranceEventRepository : RootRepository<EnduranceEvent, SetupState>
{
    public EnduranceEventRepository(IStore<SetupState> store)
        : base(store) { }
}
