using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Domain.State;

namespace EnduranceJudge.Application.Models;

public class StateContext : IStateContext, ITransientService
{
    public StateContext(IState state)
    {
        this.State = state;
    }
    
    public IState State { get; }
}
