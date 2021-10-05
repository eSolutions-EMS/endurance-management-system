using EnduranceJudge.Domain.Core.Models;

namespace EnduranceJudge.Application.Contracts
{
    public interface IStateHolder
    {
        void Update(IAggregateRoot aggregateRoot);
    }
}
