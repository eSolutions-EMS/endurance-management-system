using EnduranceJudge.Domain.State;

namespace EnduranceJudge.Domain.Core.Models
{
    public interface IAggregateRoot : IDomainModel
    {
        void UpdateState(IState state);
    }
}
