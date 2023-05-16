using EMS.Core.Application.Models;
using EMS.Core.Application.Services;
using EMS.Core.Domain.State;

namespace EMS.Core.Application.State;

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
