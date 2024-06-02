using NTS.Domain.Setup.Entities;
using NTS.Persistence.Setup;

namespace NTS.Persistence.Adapters;

public class LapRepository : SetRepository<Lap, SetupState>
{
    public LapRepository(IStore<SetupState> store) : base(store)
    {
        Store = store;
    }

    SetupState? State;
    IStore<SetupState> Store;
    public override async Task<Lap> Update(Lap entity)
    {
        await UpdateLapValue(entity);
        return entity;
    }
    public override async Task<Lap> Delete(Lap entity)
    {
        await UpdateLapValue(entity,true);
        return entity;
    }

    public async Task UpdateLapValue(Lap entity, bool isDelete=false) 
    {
        State = await Store.Load();
        for(int i = 0; i < State.Laps.Count; i++)
        {
            if (State.Laps[i] == entity)
            {
                if (isDelete)
                {
                    State.Laps.RemoveAt(i);
                }
                else
                {
                    State.Laps[i] = entity;
                }
            }
        }
        foreach (var phase in State.Event!.Competitions.SelectMany(x => x.Phases))
        {
            if (phase.Lap == entity)
            {
                if (isDelete)
                {
                    phase.Lap = null;
                }
                else
                {
                    phase.Lap = entity;
                }
            }
        }
        await Store.Commit(State);
    }
}