using Not.Storage.Repositories;
using NTS.Domain.Setup.Aggregates;

namespace NTS.Storage.Setup.Repositories;

public class OfficialRepository : SetRepository<Official, SetupState>
{
    public OfficialRepository(IStore<SetupState> store)
        : base(store) { }
}
