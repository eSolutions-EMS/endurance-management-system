using NTS.Domain.Setup.Aggregates;

namespace NTS.Storage.Setup.Repositories;

public class ParticipationRepository : SetRepository<Participation, SetupState>
{
    public ParticipationRepository(IStore<SetupState> store)
        : base(store) { }
}
