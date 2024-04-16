using Not.Application.Ports.CRUD;
using Not.Exceptions;
using NTS.Domain.Setup.Entities;

namespace NTS.Persistence.Adapters;

public class CompetitionRepository : ParentRepository<Contestant, Loop, Competition, State>, IParentRepository<Contestant>, IParentRepository<Loop>
{
    public CompetitionRepository(IStore<State> store) : base(store)
    {
    }

    public override async Task<Contestant> Update(int parentId, Contestant child)
    {
        var context = await Store.Load();
        var parent = context.Competitions.FirstOrDefault(x => x.Id == parentId);
        GuardHelper.ThrowIfNull(parent);

        parent.Update(child);
        await Store.Commit(context);

        return child;
    }

    public override async Task<Loop> Update(int parentId, Loop child)
    {
        var context = await Store.Load();
        var parent = context.Competitions.FirstOrDefault(x => x.Id == parentId);
        GuardHelper.ThrowIfNull(parent);

        parent.Update(child);
        await Store.Commit(context);

        return child;
    }
}
