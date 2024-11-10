using NTS.Domain.Setup.Entities;
using NTS.Persistence.States;

namespace NTS.Persistence.Adapters;

public class SetupParticipationRepository : SetRepository<Participation, SetupState>
{
    public SetupParticipationRepository(IStore<SetupState> store) : base(store) { }
}
