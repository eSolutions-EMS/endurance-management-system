using NTS.Domain.Setup.Entities;

namespace NTS.Persistence.Boundaries.Setup.Repositories;

public class CombinationRepository : SetRepository<Combination, SetupState>
{
    public CombinationRepository(IStore<SetupState> store)
        : base(store) { }
}
