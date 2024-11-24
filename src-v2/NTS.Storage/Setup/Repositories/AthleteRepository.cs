using NTS.Domain.Setup.Aggregates;

namespace NTS.Storage.Setup.Repositories;

public class AthleteRepository : SetRepository<Athlete, SetupState>
{
    public AthleteRepository(IStore<SetupState> store)
        : base(store) { }
}
