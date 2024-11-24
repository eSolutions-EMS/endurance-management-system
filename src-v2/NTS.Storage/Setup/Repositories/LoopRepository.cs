using NTS.Domain.Setup.Aggregates;

namespace NTS.Storage.Setup.Repositories;

public class LoopRepository : SetRepository<Loop, SetupState>
{
    public LoopRepository(IStore<SetupState> store)
        : base(store) { }
}
