using EnduranceJudge.Application.Contracts.Athletes;
using EnduranceJudge.Domain.Aggregates.Common.Athletes;
using EnduranceJudge.Gateways.Persistence.Contracts.WorkFile;
using EnduranceJudge.Gateways.Persistence.Core;
using EnduranceJudge.Gateways.Persistence.DataStores;
using EnduranceJudge.Gateways.Persistence.Entities.Athletes;

namespace EnduranceJudge.Gateways.Persistence.Contracts.Repositories
{
    //TODO: improve BaseRepository to remove the need for these.
    public class AthleteRepository : RepositoryBase<IAthleteDataStore, AthleteEntity, Athlete>,
        IAthleteCommands,
        IAthleteQueries
    {
        public AthleteRepository(IAthleteDataStore dataStore, IWorkFileUpdater workFileUpdater)
            : base(dataStore, workFileUpdater)
        {
        }
    }
}
