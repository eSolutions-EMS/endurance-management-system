using NTS.Domain.Core.Entities;

namespace NTS.Persistence.Boundaries.Core.Repositories;

public class ParticipationRepository : SetRepository<Participation, CoreState>
{
    public ParticipationRepository(IStore<CoreState> store)
        : base(store) { }
}
