using NTS.Domain.Core.Entities;
using NTS.Persistence.States;

namespace NTS.Persistence.Adapters;

public class CoreEventRepository : FlatRepository<Event, CoreState>
{
    public CoreEventRepository(IStore<CoreState> store) : base(store)
    {
    }
}
