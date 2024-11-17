using NTS.Domain.Setup.Entities;

namespace NTS.Persistence.Boundaries.Setup.Repositories;

public class HorseRepository : SetRepository<Horse, SetupState>
{
    public HorseRepository(IStore<SetupState> store)
        : base(store) { }
}
