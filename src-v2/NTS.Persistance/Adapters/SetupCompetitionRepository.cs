using NTS.Domain.Setup.Entities;
using NTS.Persistence.States;

namespace NTS.Persistence.Adapters;

public class SetupCompetitionRepository : SetRepository<Competition, SetupState>
{
    public SetupCompetitionRepository(IStore<SetupState> store) : base(store) { }
}
