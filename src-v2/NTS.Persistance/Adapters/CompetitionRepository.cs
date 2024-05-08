using Not.Domain;
using NTS.Domain.Setup.Entities;

namespace NTS.Persistence.Adapters;

public class CompetitionRepository : ChildRepository<Competition, State>
{
    public CompetitionRepository(IStore<State> store) : base(store)
    {
    }
    protected override Competition? Get(State context, int id)
    {
        return context.Event?.Competitions.FirstOrDefault(x => x.Id == id);
    }

    protected override IParent<Competition>? GetParent(State context, int childId)
    {
        return context.Event;
    }
}
