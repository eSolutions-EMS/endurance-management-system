using Not.Storage.Repositories.Adapters;
using NTS.Domain.Setup.Entities;

namespace NTS.Persistence.Boundaries.Setup.Repositories;

public class EnduranceEventRepository : RootRepository<EnduranceEvent, SetupState>
{
    public EnduranceEventRepository(IStore<SetupState> store)
        : base(store) { }
}
