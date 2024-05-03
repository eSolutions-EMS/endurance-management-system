using NTS.Domain.Setup.Entities;
using Not.Domain;
using NTS.Persistence.Setup;
using Not.Storage.Stores;

namespace NTS.Persistence.Adapters;

public class SetupCompetitionRepository : BranchRepository<Competition, SetupState>
{
    public SetupCompetitionRepository(IStore<SetupState> store) : base(store)
    {
    }

    protected override Competition? Get(SetupState state, int id)
    {
        return state.Event?.Competitions.FirstOrDefault(x => x.Id == id);
    }

    protected override IParent<Competition>? GetParent(SetupState state, int childId)
    {
        return state.Event;
    }
}
