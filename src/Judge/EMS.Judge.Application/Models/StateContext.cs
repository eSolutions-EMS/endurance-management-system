using EMS.Core.Domain.State;

namespace EMS.Judge.Application.Models;

public class StateContext : IStateContext
{
    public StateContext(IState state)
    {
        this.State = state;
    }
    
    public IState State { get; }
}
