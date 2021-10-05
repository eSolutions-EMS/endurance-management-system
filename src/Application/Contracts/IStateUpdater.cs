using EnduranceJudge.Domain.Core.Models;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Contracts
{
    public interface IStateUpdater
    {
        Task Update(IAggregateRoot aggregateRoot);
    }
}
