using NTS.Domain.Setup.Entities;
using NTS.Persistence.Setup;

namespace NTS.Persistence.Adapters;
public class CombinationRepository : SetRepository<Combination, SetupState>
{
    private readonly IStore<SetupState> _store;

    public CombinationRepository(IStore<SetupState> store) : base(store)
    {
        _store = store;
    }

    public override async Task<Combination> Create(Combination combination)
    {
        var state = await _store.Transact();
        MatchReferences(state, combination);
        
        state.Combinations.Add(combination);

        await _store.Commit(state);
        return combination;
    }

    public override async Task<Combination> Update(Combination combination)
    {
        var state = await _store.Transact();
        MatchReferences(state, combination);
        
        var existing = state.Combinations.FirstOrDefault(x => x == combination);
        if (existing != null)
        {
            state.Combinations.Remove(existing);
        }
        state.Combinations.Add(combination);
        
        await _store.Commit(state);
        return combination;
    }

    private void MatchReferences(SetupState state, Combination combination)
    {
        var athleteMatch = state.Athletes.FirstOrDefault(x => x == combination.Athlete);
        if (athleteMatch != null)
        {
            combination.Athlete = athleteMatch;
        }
        var horseMatch = state.Horses.FirstOrDefault(x => x == combination.Horse);
        if (horseMatch != null)
        {
            combination.Horse = horseMatch;
        }
    }
}
