using Endurance.Judge.Gateways.API.Models;
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
        
        public void Execute(JudgeEvent judgeEvent)
        {
            try
            {
                this.InnerExecute(judgeEvent);
            }
            catch (Exception exception)
            {
                this.logger.LogEventError(exception, judgeEvent);
            }
        }

        private void InnerExecute(JudgeEvent judgeEvent)
        {
            var manager = new ManagerRoot();
            switch (judgeEvent.Type)
            {
                case JudgeEventType.Finish: manager.RecordArrive(judgeEvent.TagId, judgeEvent.Time);
                    break;
                case JudgeEventType.EnterVet: manager.RecordInspect(judgeEvent.TagId, judgeEvent.Time);
                    break;
                case JudgeEventType.Invalid:
                default:
                    break;
            }
        }
    }
    
    public interface IJudgeEventExecutor : ITransientService
    {
        void Execute(JudgeEvent judgeEvent);
    }
}
