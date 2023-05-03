using EnduranceJudge.Application.Models;
using EnduranceJudge.Application.Services;
using EnduranceJudge.Domain.State;

namespace EnduranceJudge.Application.State;

public class WitnessAwareContext : StateContext, IWitnessAwareContext
{
    public WitnessAwareContext(IWitnessPollingService witnessPollingService, IState state)
        : base(state)
    {
        witnessPollingService.ApplyEvents();
    }
}

public interface IWitnessAwareContext : IStateContext
{
}
