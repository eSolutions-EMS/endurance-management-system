using NTS.Domain.Core.Entities;
using NTS.Persistence.States;

namespace NTS.Persistence.Adapters;

public class HandoutRepository : SetRepository<Handout, CoreState>
{
    public HandoutRepository(IStore<CoreState> store)
        : base(store) { }
}
