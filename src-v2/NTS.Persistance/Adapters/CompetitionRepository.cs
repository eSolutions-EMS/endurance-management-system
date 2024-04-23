using NTS.Domain.Setup.Entities;
using Not.Domain;

namespace NTS.Persistence.Adapters;

public class CompetitionRepository : ChildRepository<Competition, SetupContext>
{
    public CompetitionRepository(IStore<SetupContext> store) : base(store)
    {
    }

    protected override Competition? Get(SetupContext context, int id)
    {
        return context.Event?.Competitions.FirstOrDefault(x => x.Id == id);
    }

    protected override IParent<Competition>? GetParent(SetupContext context, int childId)
    {
        return context.Event;
    }
}
