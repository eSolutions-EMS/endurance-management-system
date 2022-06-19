using Endurance.Judge.Gateways.API.Models;
using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Domain.AggregateRoots.Manager;
using System;

namespace Endurance.Judge.Gateways.API.Services
{
    public class JudgeEventExecutor : IJudgeEventExecutor
    {
        public JudgeEventExecutor()
        {
            new ManagerRoot();
        }
        
        public void Execute(JudgeEvent judgeEvent)
        {
            try
            {
                this.InnerExecute(judgeEvent);
            }
            catch (Exception exception)
            {
                // TODO: proper handling. Some sort of notification.
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
