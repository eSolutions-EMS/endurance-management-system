using NTS.Domain.Setup.Entities;

namespace NTS.Storage.Boundaries.Setup.Repositories;

public class HorseRepository : SetRepository<Horse, SetupState>
{
    public HorseRepository(IStore<SetupState> store)
        : base(store) { }
}
