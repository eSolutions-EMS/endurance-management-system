using Not.Storage.Repositories;
using NTS.Domain.Setup.Aggregates;

namespace NTS.Storage.Setup.Repositories;

public class CompetitionRepository : SetRepository<Competition, SetupState>
{
    public CompetitionRepository(IStore<SetupState> store)
        : base(store) { }
}
