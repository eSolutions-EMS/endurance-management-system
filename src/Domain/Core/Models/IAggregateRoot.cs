using EnduranceJudge.Domain.State;

namespace EnduranceJudge.Domain.Core.Models
{
    public interface IAggregateRoot : IDomainObject
    {
        void UpdateState(IState state);
    }
}
