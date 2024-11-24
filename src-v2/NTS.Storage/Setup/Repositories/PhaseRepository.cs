using NTS.Domain.Setup.Entities;

namespace NTS.Storage.Boundaries.Setup.Repositories;

public class PhaseRepository : SetRepository<Phase, SetupState>
{
    public PhaseRepository(IStore<SetupState> store)
        : base(store) { }
}
