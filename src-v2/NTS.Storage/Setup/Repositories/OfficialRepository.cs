using NTS.Domain.Setup.Entities;

namespace NTS.Storage.Boundaries.Setup.Repositories;

public class OfficialRepository : SetRepository<Official, SetupState>
{
    public OfficialRepository(IStore<SetupState> store)
        : base(store) { }
}
