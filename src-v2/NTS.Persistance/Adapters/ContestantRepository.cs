using NTS.Domain.Setup.Entities;
using Not.Exceptions;
using Not.Application.Ports.CRUD;

namespace NTS.Persistence.Adapters;

public class ContestantRepository : ParentRepository<Contestant, Competition, State>, IParentRepository<Contestant>
{
    public ContestantRepository(IStore<State> store) : base(store)
    {
    }

    public async override Task<Competition> Update(Competition entity)
    {
        var context = await Store.Load();
        GuardHelper.ThrowIfNull(context.Event);

        foreach (var contestant in context.Contestants)
        {
            entity.Add(contestant);
        }

        await Store.Commit(context);

        return entity;
    }
    public async Task<Contestant> Update(Contestant entity)
    {
        var context = await Store.Load();
        var existing = context.Contestants.FirstOrDefault();
        GuardHelper.ThrowIfNull(existing);

        context.Contestants.Remove(entity);
        context.Contestants.Add(entity);
        await Store.Commit(context);

        return entity;
    }
}
