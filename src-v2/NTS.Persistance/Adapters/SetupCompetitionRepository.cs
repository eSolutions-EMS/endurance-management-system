using NTS.Domain.Setup.Entities;
using Not.Domain;
using NTS.Persistence.Setup;

namespace NTS.Persistence.Adapters;

public class SetupCompetitionRepository : ChildRepository<Competition, SetupContext>
{
    public SetupCompetitionRepository(IStore<SetupContext> store) : base(store)
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
