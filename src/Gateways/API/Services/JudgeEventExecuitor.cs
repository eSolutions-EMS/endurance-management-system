using Endurance.Judge.Gateways.API.Models;
using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Domain.AggregateRoots.Manager;

namespace Endurance.Judge.Gateways.API.Services
{
    public class JudgeEventExecutor : IJudgeEventExecutor
    {
        private readonly ManagerRoot manager;
        public JudgeEventExecutor(ManagerRoot manager)
        {
            this.manager = manager;
        }
        
        public void Execute(JudgeEvent judgeEvent)
        {
            switch (judgeEvent.Type)
            {
                case JudgeEventType.Finish: this.manager.RecordArrive(judgeEvent.TagId, judgeEvent.Time);
                    break;
                case JudgeEventType.EnterVet: this.manager.RecordInspect(judgeEvent.TagId, judgeEvent.Time);
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
