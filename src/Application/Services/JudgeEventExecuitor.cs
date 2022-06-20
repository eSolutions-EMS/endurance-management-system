using EnduranceJudge.Application.Models;
using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Domain.AggregateRoots.Manager;
using System;

namespace Endurance.Judge.Gateways.API.Services
{
    public class JudgeEventExecutor : IJudgeEventExecutor
    {
        private readonly ILogger logger;
        public JudgeEventExecutor(ILogger logger)
        {
            this.logger = logger;
        }
        
        public void Execute(WitnessEvent witnessEvent)
        {
            try
            {
                this.InnerExecute(witnessEvent);
            }
            catch (Exception exception)
            {
                this.logger.LogEventError(exception, witnessEvent);
            }
        }

        private void InnerExecute(WitnessEvent witnessEvent)
        {
            var manager = new ManagerRoot();
            switch (witnessEvent.Type)
            {
                case WitnessEventType.Finish: manager.RecordArrive(witnessEvent.TagId, witnessEvent.Time);
                    break;
                case WitnessEventType.EnterVet: manager.RecordInspect(witnessEvent.TagId, witnessEvent.Time);
                    break;
                case WitnessEventType.Invalid:
                default:
                    break;
            }
        }
    }
    
    public interface IJudgeEventExecutor : ITransientService
    {
        void Execute(WitnessEvent witnessEvent);
    }
}
