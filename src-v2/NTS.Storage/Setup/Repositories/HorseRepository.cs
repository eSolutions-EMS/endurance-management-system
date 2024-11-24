using Not.Storage.Repositories;
using NTS.Domain.Setup.Aggregates;

namespace NTS.Storage.Setup.Repositories;

public class HorseRepository : SetRepository<Horse, SetupState>
{
    public HorseRepository(IStore<SetupState> store)
        : base(store) { }
}
