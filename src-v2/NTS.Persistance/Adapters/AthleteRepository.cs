using NTS.Domain.Setup.Entities;
using NTS.Persistence.States;

namespace NTS.Persistence.Adapters;

public class AthleteRepository : SetRepository<Athlete, SetupState>
{
    public AthleteRepository(IStore<SetupState> store) : base(store) { }
}
