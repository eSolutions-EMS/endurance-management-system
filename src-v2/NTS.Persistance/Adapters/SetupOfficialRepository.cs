using NTS.Domain.Setup.Entities;
using NTS.Persistence.States;

namespace NTS.Persistence.Adapters;

public class SetupOfficialRepository : SetRepository<Official, SetupState>
{
    public SetupOfficialRepository(IStore<SetupState> store) : base(store) { }
}
