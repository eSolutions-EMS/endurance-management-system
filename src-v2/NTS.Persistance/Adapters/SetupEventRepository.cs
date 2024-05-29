using NTS.Domain.Setup.Entities;
using NTS.Persistence.Setup;

namespace NTS.Persistence.Adapters;

public class SetupEventRepository : TreeRepository<Event, SetupState>
{
    public SetupEventRepository(IStore<SetupState> store) : base(store)
    {
    }
}