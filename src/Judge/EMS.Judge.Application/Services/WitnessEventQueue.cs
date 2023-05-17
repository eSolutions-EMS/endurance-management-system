using EMS.Judge.Application.Models;
using Core.ConventionalServices;
using Core.Domain.AggregateRoots.Manager;
using Core.Domain.AggregateRoots.Manager.WitnessEvents;
using System.Collections.Generic;
using System.Linq;

namespace EMS.Judge.Application.Services
{
    public class WitnessEventQueue : IWitnessEventQueue
    {
        private readonly Queue<WitnessEvent> events = new();

        public WitnessEventQueue()
        {
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
                Witness.Raise(judgeEvent);
            }
        }
    }

    public interface IWitnessEventQueue : ISingletonService
    {
        void AddEvent(WitnessEvent witnessEvent);
        void ApplyEvents();
    }
}
