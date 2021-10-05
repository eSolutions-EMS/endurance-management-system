using EnduranceJudge.Domain.Core.Models;

namespace EnduranceJudge.Application.Contracts
{
    public interface IStateUpdater
    {
        void Update(IAggregateRoot aggregateRoot);
    }
}
