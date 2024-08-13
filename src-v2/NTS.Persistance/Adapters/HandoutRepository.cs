using NTS.Domain.Core.Objects;
using NTS.Persistence.States;

namespace NTS.Persistence.Adapters;

public class HandoutRepository : SetRepository<HandoutDocument, CoreState>
{
    public HandoutRepository(IStore<CoreState> store) : base(store)
    {
    }
}
