using NTS.Domain.Core.Entities;
using NTS.Persistence.States;

namespace NTS.Persistence.Adapters;

public class CoreEventRepository : RootRepository<Event, CoreState>
{
    public CoreEventRepository(IStore<CoreState> store) : base(store)
    {
    }
}
