using NTS.Domain.Setup.Entities;
using NTS.Persistence.States;

namespace NTS.Persistence.Adapters;

public class LoopRepository : SetRepository<Loop, SetupState>
{
    public LoopRepository(IStore<SetupState> store)
        : base(store) { }
}
