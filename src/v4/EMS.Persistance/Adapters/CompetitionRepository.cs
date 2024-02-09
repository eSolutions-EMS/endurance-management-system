using Common.Helpers;
using EMS.Domain.Setup.Entities;

namespace EMS.Persistence.Adapters;

public class CompetitionRepository : RepositoryBase<Competition, State>
{
    public CompetitionRepository(IStore<State> store) : base(store)
    {
    }

    public override async Task<Competition> Update(Competition entity)
    {
        var context = await Store.Load();
        var existing = context.Competitions.FirstOrDefault();
        ThrowHelper.ThrowIfNull(existing);

        context.Competitions.Remove(entity);
        context.Competitions.Add(entity);
        await Store.Commit(context);

        return entity;
    }
}
