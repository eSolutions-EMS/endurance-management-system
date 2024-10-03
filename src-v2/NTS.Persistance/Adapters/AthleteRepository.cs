using NTS.Domain.Setup.Entities;
using NTS.Persistence.Exceptions;
using NTS.Persistence.Setup;

namespace NTS.Persistence.Adapters;

public class AthleteRepository : SetRepository<Athlete, SetupState>
{
    public AthleteRepository(IStore<SetupState> store) : base(store)
    {
    }

    public override async Task<Athlete> Delete(Athlete entity)
    {
        var state = await Store.Transact();
        var parent = state.Combinations.FirstOrDefault(x => x.Athlete == entity);
        if (parent != null)
        {
            throw new ParentalViolationException($"'{entity}' cannot be deleted because it's contained in '{parent}'");
        }
        return entity;
    }

    protected override void PerformUpdate(SetupState state, Athlete entity)
    {
        var match = state.Athletes.First(x => x == entity);
        match.Person = entity.Person;
        match.Club = entity.Club;
        match.Category = entity.Category;
        match.Country = entity.Country;
        match.FeiId = entity.FeiId;
    }
}