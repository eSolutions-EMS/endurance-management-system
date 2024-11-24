using NTS.Domain.Setup.Entities;

namespace NTS.Storage.Boundaries.Setup.Repositories;

public class ParticipationRepository : SetRepository<Participation, SetupState>
{
    public ParticipationRepository(IStore<SetupState> store)
        : base(store) { }
}
