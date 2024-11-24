using NTS.Domain.Core.Entities;
using NTS.Storage.Boundaries.Core;

namespace NTS.Storage.Boundaries.Core.Repositories;

public class ParticipationRepository : SetRepository<Participation, CoreState>
{
    public ParticipationRepository(IStore<CoreState> store)
        : base(store) { }
}
