using NTS.Domain.Setup.Entities;
using NTS.Persistence.Setup;

namespace NTS.Persistence.Adapters;

public class SetupLapRepository : SetRepository<Lap, SetupState>
{
    public SetupLapRepository(IStore<SetupState> store) : base(store)
    {
    }
}