using Core.Domain.State;
using EMS.Judge.Application.Models;
using EMS.Judge.Application.Services;

namespace EMS.Judge.Application.State;

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
