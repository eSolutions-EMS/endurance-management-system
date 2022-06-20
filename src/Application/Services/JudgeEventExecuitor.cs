using EnduranceJudge.Application.Models;
using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Domain.AggregateRoots.Manager;
using System;

namespace EnduranceJudge.Application.Services;

// TODO: rename file
public class WitnessEventExecutor : IWitnessEventExecutor
{
    public WitnessEventExecutor()
    {
    }
    
    public void Execute(WitnessEvent witnessEvent)
    {
        try
        {
            this.InnerExecute(witnessEvent);
        }
        catch (Exception exception) 
        {
            // Throw WitnessException
        }
    }

    // TODO: create WitnessRoot class and maybe remove this file completely
    private void InnerExecute(WitnessEvent witnessEvent)
    {
        switch (witnessEvent.Type)
        {
            // case WitnessEventType.Finish: this.manager.RecordArrive(witnessEvent.TagId, witnessEvent.Time);
            //     break;
            // case WitnessEventType.EnterVet: this.manager.RecordInspect(witnessEvent.TagId, witnessEvent.Time);
            //     break;
            case WitnessEventType.Invalid:
            default:
                break;
        }
    }
}
    
public interface IWitnessEventExecutor : ITransientService
{
    void Execute(WitnessEvent witnessEvent);
}
