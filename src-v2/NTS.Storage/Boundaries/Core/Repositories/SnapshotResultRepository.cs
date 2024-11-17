using NTS.Domain.Core.Entities;
using NTS.Storage.Boundaries.Core;

namespace NTS.Storage.Boundaries.Core.Repositories;

public class SnapshotResultRepository : SetRepository<SnapshotResult, CoreState>
{
    public SnapshotResultRepository(IStore<CoreState> store)
        : base(store) { }
}
