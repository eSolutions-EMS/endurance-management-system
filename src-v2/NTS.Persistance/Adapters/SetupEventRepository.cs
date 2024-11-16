using Not.Storage.Repositories.Adapters;
using NTS.Domain.Setup.Entities;
using NTS.Persistence.States;

namespace NTS.Persistence.Adapters;

public class SetupEventRepository : RootRepository<EnduranceEvent, SetupState>
{
    public SetupEventRepository(IStore<SetupState> store)
        : base(store) { }
}
