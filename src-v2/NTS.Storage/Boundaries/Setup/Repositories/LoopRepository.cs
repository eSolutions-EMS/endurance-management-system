using NTS.Domain.Setup.Entities;

namespace NTS.Storage.Boundaries.Setup.Repositories;

public class LoopRepository : SetRepository<Loop, SetupState>
{
    public LoopRepository(IStore<SetupState> store)
        : base(store) { }
}
