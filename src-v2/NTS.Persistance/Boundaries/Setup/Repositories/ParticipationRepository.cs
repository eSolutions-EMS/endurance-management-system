using NTS.Domain.Setup.Entities;

namespace NTS.Persistence.Boundaries.Setup.Repositories;

public class ParticipationRepository : SetRepository<Participation, SetupState>
{
    public ParticipationRepository(IStore<SetupState> store)
        : base(store) { }
}
