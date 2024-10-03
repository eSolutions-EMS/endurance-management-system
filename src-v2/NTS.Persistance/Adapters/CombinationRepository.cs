using NTS.Domain.Setup.Entities;
using NTS.Persistence.Exceptions;
using NTS.Persistence.Setup;

namespace NTS.Persistence.Adapters;
public class CombinationRepository : SetRepository<Combination, SetupState>
{
    public CombinationRepository(IStore<SetupState> store) : base(store)
    {
    }

    public override async Task<Combination> Create(Combination combination)
    {
        var state = await Store.Transact();
        MatchReferences(state, combination);
        
        state.Combinations.Add(combination);

        await Store.Commit(state);
        return combination;
    }

    public override async Task<Combination> Update(Combination combination)
    {
        var state = await Store.Transact();
        MatchReferences(state, combination);

        PerformUpdate(state, combination);
        
        await Store.Commit(state);
        return combination;
    }

    public override async Task<Combination> Delete(Combination entity)
    {
        var state = await Store.Transact();
        var parent = state.Event!.Competitions.SelectMany(x => x.Contestants).FirstOrDefault(x => x.Combination == entity);
        if (parent != null)
        {
            throw new ParentalViolationException($"Loop '{entity}' cannot be deleted because it's contained in '{parent}'");
        }
        return entity;
    }

    protected override void PerformUpdate(SetupState state, Combination entity)
    {
        var match = state.Combinations.First(x => x == entity);
        match.Number = entity.Number;
        match.Athlete = entity.Athlete;
        match.Horse = entity.Horse;
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
