using NTS.Domain.Core.Entities;

namespace NTS.Persistence.Boundaries.Core.Repositories;

public class SnapshotResultRepository : SetRepository<SnapshotResult, CoreState>
{
    public SnapshotResultRepository(IStore<CoreState> store)
        : base(store) { }
}
