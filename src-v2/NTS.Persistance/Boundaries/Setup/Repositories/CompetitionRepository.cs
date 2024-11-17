using NTS.Domain.Setup.Entities;

namespace NTS.Persistence.Boundaries.Setup.Repositories;

public class CompetitionRepository : SetRepository<Competition, SetupState>
{
    public CompetitionRepository(IStore<SetupState> store)
        : base(store) { }
}
