using NTS.Domain.Setup.Entities;
using NTS.Persistence.Exceptions;
using NTS.Persistence.Setup;

namespace NTS.Persistence.Adapters;

public class LoopRepository : SetRepository<Loop, SetupState>
{
    public LoopRepository(IStore<SetupState> store) : base(store)
    {
    }

    public override async Task<Loop> Delete(Loop entity)
    {
        var state = await Store.Transact();
        var parent = state.Event!.Competitions.SelectMany(x => x.Phases).FirstOrDefault(x => x.Loop == entity);
        if (parent != null)
        {
            throw new ParentalViolationException($"Loop '{entity}' cannot be deleted because it's contained in '{parent}'");
        }
        return entity;
    }

    protected override void PerformUpdate(SetupState state, Loop entity)
    {
        var match = state.Loops.First(x => x == entity);
        match.Distance = entity.Distance;
    }
}