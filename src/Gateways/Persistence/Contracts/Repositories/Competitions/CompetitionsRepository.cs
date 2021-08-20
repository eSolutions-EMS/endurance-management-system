using EnduranceJudge.Domain.Aggregates.Event.Competitions;
using EnduranceJudge.Gateways.Persistence.Contracts.WorkFile;
using EnduranceJudge.Gateways.Persistence.Core;
using EnduranceJudge.Gateways.Persistence.Entities.Competitions;

namespace EnduranceJudge.Gateways.Persistence.Contracts.Repositories.Competitions
{
    public class CompetitionsRepository : RepositoryBase<ICompetitionsDataStore, CompetitionEntity, Competition>
    {
        public CompetitionsRepository(ICompetitionsDataStore dataStore, IWorkFileUpdater workFileUpdater)
            : base(dataStore, workFileUpdater)
        {
        }
    }
}
