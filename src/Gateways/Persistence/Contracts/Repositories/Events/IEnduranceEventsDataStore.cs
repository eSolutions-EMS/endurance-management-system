using EnduranceJudge.Gateways.Persistence.Core;
using EnduranceJudge.Gateways.Persistence.Entities.Competitions;
using EnduranceJudge.Gateways.Persistence.Entities.EnduranceEvents;
using Microsoft.EntityFrameworkCore;

namespace EnduranceJudge.Gateways.Persistence.Contracts.Repositories.Events
{
    internal interface IEnduranceEventsDataStore : IDataStore
    {
        DbSet<EnduranceEventEntity> EnduranceEvents { get; }

        DbSet<CompetitionEntity> Competitions { get; }
    }
}
