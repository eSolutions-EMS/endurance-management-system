using EnduranceJudge.Application.Core.Contracts;
using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Domain.Aggregates.Manager.Participations;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Actions.Manager.Contracts
{
    public interface IParticipationQueries : IQueries<Participation>, IService
    {
        Task<Participation> GetBy(int number);
    }
}
