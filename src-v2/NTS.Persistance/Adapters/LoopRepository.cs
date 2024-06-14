using NTS.Domain.Setup.Entities;
using NTS.Persistence.Setup;

namespace NTS.Persistence.Adapters;

public class LoopRepository : SetRepository<Loop, SetupState>
{
    public LoopRepository(IStore<SetupState> store) : base(store)
    {
        Store = store;
    }

    SetupState? State;
    IStore<SetupState> Store;
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

    public async Task UpdateLoopValue(Loop entity, bool isDelete=false) 
    {
        State = await Store.Load();
        for(int i = 0; i < State.Loops.Count; i++)
        {
            if (State.Loops[i] == entity)
            {
                if (isDelete)
                {
                    State.Loops.RemoveAt(i);
                }
                else
                {
                    State.Loops[i] = entity;
                }
            }
        }
        foreach (var phase in State.Event!.Competitions.SelectMany(x => x.Phases))
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
        await Store.Commit(State);
    }
}