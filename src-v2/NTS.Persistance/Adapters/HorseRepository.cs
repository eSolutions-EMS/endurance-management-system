using NTS.Domain.Setup.Entities;
using NTS.Persistence.Exceptions;
using NTS.Persistence.Setup;

namespace NTS.Persistence.Adapters;
public class HorseRepository : SetRepository<Horse, SetupState>
{
    public HorseRepository(IStore<SetupState> store) : base(store)
    {
    }

    public override async Task<Horse> Delete(Horse entity)
    {
        var state = await Store.Transact();
        var parent = state.Combinations.FirstOrDefault(x => x.Horse == entity);
        if (parent != null)
        {
            throw new ParentalViolationException($"'{entity}' cannot be deleted because it's contained in '{parent}'");
        }
        return entity;
    }

    protected override void PerformUpdate(SetupState state, Horse entity)
    {
        var match = state.Horses.First(x => x == entity);
        match.Name = entity.Name;
        match.FeiId = entity.FeiId;
    }
}
