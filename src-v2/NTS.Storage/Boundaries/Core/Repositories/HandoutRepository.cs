using NTS.Domain.Core.Entities;
using NTS.Storage.Boundaries.Core;

namespace NTS.Storage.Boundaries.Core.Repositories;

public class HandoutRepository : SetRepository<Handout, CoreState>
{
    public HandoutRepository(IStore<CoreState> store)
        : base(store) { }
}
