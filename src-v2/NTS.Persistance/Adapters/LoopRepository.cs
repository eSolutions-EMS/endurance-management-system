using NTS.Domain.Setup.Entities;
using NTS.Persistence.Setup;

namespace NTS.Persistence.Adapters;

public class LoopRepository : SetRepository<Loop, SetupState>
{
    private readonly IStore<SetupState> _store;

    public LoopRepository(IStore<SetupState> store) : base(store)
    {
        _store = store;
    }

    public override async Task<Loop> Update(Loop entity)
    {
        await UpdateLoopValue(entity);
        return entity;
    }
    public override async Task<Loop> Delete(Loop entity)
    {
        await UpdateLoopValue(entity,true);
        return entity;
    }

    private async Task UpdateLoopValue(Loop entity, bool isDelete=false) 
    {
        var state = await _store.Transact();
        for(int i = 0; i < state.Loops.Count; i++)
        {
            if (state.Loops[i] == entity)
            {
                if (isDelete)
                {
                    state.Loops.RemoveAt(i);
                }
                else
                {
                    state.Loops[i] = entity;
                }
            }
        }
        foreach (var phase in state.Event!.Competitions.SelectMany(x => x.Phases))
        {
            if (phase.Loop == entity)
            {
                if (isDelete)
                {
                    phase.Loop = null;
                }
                else
                {
                    phase.Loop = entity;
                }
            }
        }
        await _store.Commit(state);
    }
}