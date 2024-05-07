using NTS.Domain.Core.Aggregates.Participations;
using NTS.Persistence.States;

namespace NTS.Persistence.Adapters;

public class CoreParticipationRepository : SetRepository<Participation, CoreState>
{
    public CoreParticipationRepository(IStore<CoreState> store) : base(store)
    {
    }
}
