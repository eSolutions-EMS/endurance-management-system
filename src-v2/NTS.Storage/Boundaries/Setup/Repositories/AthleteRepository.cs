using NTS.Domain.Setup.Entities;

namespace NTS.Storage.Boundaries.Setup.Repositories;

public class AthleteRepository : SetRepository<Athlete, SetupState>
{
    public AthleteRepository(IStore<SetupState> store)
        : base(store) { }
}
