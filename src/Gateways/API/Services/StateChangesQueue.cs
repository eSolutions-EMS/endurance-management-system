using EnduranceJudge.Application;
using EnduranceJudge.Core.ConventionalServices;
using System.Collections.Generic;
using System.Linq;

namespace Endurance.Judge.Gateways.API.Services
{
    public class StateChangesQueue : IStateChangesQueue
    {
        private readonly List<State> queue = new();
        
        public void Enqueue(State state)
        {
            this.queue.Add(state);
        }

        public State Dequeue()
        {
            var state = this.queue.FirstOrDefault();
            if (state != null)
            {
                this.queue.Remove(state);
            }
            return state;
        }
    }

    public interface IStateChangesQueue : ISingletonService
    {
        void Enqueue(State state);
        State Dequeue();
    }
}
