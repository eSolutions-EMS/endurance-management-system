using NTS.Domain.Setup.Entities;
using NTS.Persistence.States;

namespace NTS.Persistence.Adapters;

public class CombinationRepository : SetRepository<Combination, SetupState>
{
    public CombinationRepository(IStore<SetupState> store)
        : base(store) { }
}
