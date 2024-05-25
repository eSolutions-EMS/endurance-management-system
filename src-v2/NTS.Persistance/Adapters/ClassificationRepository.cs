using NTS.Domain.Core.Entities;
using NTS.Persistence.States;

namespace NTS.Persistence.Adapters;

public class ClassificationRepository : SetRepository<Classification, CoreState>
{
    public ClassificationRepository(IStore<CoreState> store) : base(store)
    {
    }
}
