using EnduranceJudge.Domain.State;

namespace EnduranceJudge.Domain.Core.Models
{
    public interface IAggregateRoot
    {
        void UpdateState(IState state);
    }
}
