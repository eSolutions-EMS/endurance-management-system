using EnduranceJudge.Application.Models;
using EnduranceJudge.Core.ConventionalServices;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Application.Services
{
    public class WitnessEventQueue : IWitnessEventQueue
    {
        private readonly IWitnessEventExecutor eventExecutor;
        private readonly Queue<WitnessEvent> events = new();

        public WitnessEventQueue(IWitnessEventExecutor eventExecutor)
        {
            this.eventExecutor = eventExecutor;
        }
        
        public void AddEvent(WitnessEvent witnessEvent)
        {
            this.events.Enqueue(witnessEvent);
        }

        public void ApplyEvents()
        {
            while (this.events.Any())
            {
                var judgeEvent = this.events.Dequeue();
                this.eventExecutor.Execute(judgeEvent);
            }
        }
    }

    public interface IWitnessEventQueue : ISingletonService
    {
        void AddEvent(WitnessEvent witnessEvent);
        void ApplyEvents();
    }
}
