using NTS.Domain.Setup.Entities;

namespace NTS.Storage.Boundaries.Setup.Repositories;

public class CompetitionRepository : SetRepository<Competition, SetupState>
{
    public CompetitionRepository(IStore<SetupState> store)
        : base(store) { }
}
