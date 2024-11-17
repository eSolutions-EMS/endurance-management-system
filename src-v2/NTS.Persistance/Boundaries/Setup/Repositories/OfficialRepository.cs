using NTS.Domain.Setup.Entities;

namespace NTS.Persistence.Boundaries.Setup.Repositories;

public class OfficialRepository : SetRepository<Official, SetupState>
{
    public OfficialRepository(IStore<SetupState> store)
        : base(store) { }
}
