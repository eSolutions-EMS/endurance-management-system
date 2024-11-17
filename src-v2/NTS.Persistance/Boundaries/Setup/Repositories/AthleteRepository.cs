using NTS.Domain.Setup.Entities;

namespace NTS.Persistence.Boundaries.Setup.Repositories;

public class AthleteRepository : SetRepository<Athlete, SetupState>
{
    public AthleteRepository(IStore<SetupState> store)
        : base(store) { }
}
