
using NTS.Domain.Setup.Entities;
using NTS.Persistence.Setup;

namespace NTS.Persistence.Adapters;
public class AthleteRepository : SetRepository<Athlete, SetupState>
{
    public AthleteRepository(IStore<SetupState> store) : base(store)
    {
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