using EnduranceJudge.Domain.Core.Models;

namespace EnduranceJudge.Application.Contracts
{
    public interface IPersistence
    {
        void Update(IAggregateRoot aggregateRoot);
    }
}
