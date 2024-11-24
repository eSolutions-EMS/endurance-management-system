using Not.Storage.Repositories;
using NTS.Domain.Setup.Aggregates;

namespace NTS.Storage.Setup.Repositories;

public class CombinationRepository : SetRepository<Combination, SetupState>
{
    public CombinationRepository(IStore<SetupState> store)
        : base(store) { }
}
