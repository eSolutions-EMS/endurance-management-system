using EnduranceJudge.Application.Contracts;
using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Domain.Core.Models;

namespace EnduranceJudge.Gateways.Persistence.Contracts
{
    public class StateHolder : IStateHolder, ISingletonService
    {
        private readonly State state;

        public StateHolder()
        {
            this.state = new State();
        }

        public void Update(IAggregateRoot aggregateRoot)
        {
            aggregateRoot.UpdateState(this.state);
        }
    }
}
