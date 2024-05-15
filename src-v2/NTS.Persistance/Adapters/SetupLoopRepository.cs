using NTS.Domain.Setup.Entities;
using NTS.Persistence.Setup;

namespace NTS.Persistence.Adapters;

public class SetupPhaseRepository : RootRepository<Phase, SetupState>
{
    public SetupPhaseRepository(IStore<SetupState> store) : base(store)
    {
    }
}