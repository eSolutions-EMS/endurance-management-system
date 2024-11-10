using NTS.Domain.Core.Entities;
using NTS.Persistence.States;

namespace NTS.Persistence.Adapters;

public class CoreOfficialRepository : SetRepository<Official, CoreState>
{
    public CoreOfficialRepository(IStore<CoreState> store) : base(store) { }
}
