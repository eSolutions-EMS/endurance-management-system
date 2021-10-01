using EnduranceJudge.Application.Actions.Manager.Contracts;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Aggregates.Manager.Participations;
using EnduranceJudge.Gateways.Persistence.Contracts.WorkFile;
using EnduranceJudge.Gateways.Persistence.Core;
using EnduranceJudge.Gateways.Persistence.Entities.Participants;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EnduranceJudge.Gateways.Persistence.Contracts.Repositories.Manager
{
    public class ParticipantRepository : Repository<ParticipantEntity, Participation>, IParticipationQueries
    {
        public ParticipantRepository(EnduranceJudgeDbContext dbContext, IWorkFileUpdater workFileUpdater)
            : base(dbContext, workFileUpdater)
        {
        }

        public async Task<Participation> GetBy(int number)
        {
            var participant = await this.DbContext
                .Set<ParticipantEntity>()
                .Where(p => p.Number == number)
                .MapQueryable<Participation>()
                .FirstOrDefaultAsync();
            return participant;
        }
    }
}
