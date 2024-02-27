using NTS.Domain.Setup.Entities;
using Not.Exceptions;

namespace NTS.Persistence.Adapters;

public class CompetitionRepository : RepositoryBase<Competition, State>
{
    public CompetitionRepository(IStore<State> store) : base(store)
    {
    }

    public override async Task<Competition> Update(Competition entity)
    {
        var context = await Store.Load();
        var existing = context.Competitions.FirstOrDefault();
        GuardHelper.ThrowIfNull(existing);

        context.Competitions.Remove(entity);
        context.Competitions.Add(entity);
        await Store.Commit(context);

        return entity;
    }
}
