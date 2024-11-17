using NTS.Domain.Setup.Entities;

namespace NTS.Persistence.Boundaries.Setup.Repositories;

public class PhaseRepository : SetRepository<Phase, SetupState>
{
    public PhaseRepository(IStore<SetupState> store)
        : base(store) { }
}
