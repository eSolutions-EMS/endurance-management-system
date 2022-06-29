using EnduranceJudge.Application.State;
using EnduranceJudge.Domain.State;

namespace EnduranceJudge.Application.Services;

public interface IStateSetter : IState
{
    internal void Set(StateModel initial);
}
