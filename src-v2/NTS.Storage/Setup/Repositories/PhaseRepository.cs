using NTS.Domain.Setup.Aggregates;

namespace NTS.Storage.Setup.Repositories;

public class PhaseRepository : SetRepository<Phase, SetupState>
{
    public PhaseRepository(IStore<SetupState> store)
        : base(store) { }
}
