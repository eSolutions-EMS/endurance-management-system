using NTS.Domain.Setup.Entities;
using NTS.Persistence.States;

namespace NTS.Persistence.Adapters;

public class SetupPhaseRepository : SetRepository<Phase, SetupState>
{
    public SetupPhaseRepository(IStore<SetupState> store) : base(store) { }
}
