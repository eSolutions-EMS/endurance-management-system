using EnduranceJudge.Domain.State;

namespace EnduranceJudge.Application.Models;

public class StateContext : IStateContext
{
    public StateContext(IState state)
    {
        this.State = state;
    }
    
    public IState State { get; }
}
