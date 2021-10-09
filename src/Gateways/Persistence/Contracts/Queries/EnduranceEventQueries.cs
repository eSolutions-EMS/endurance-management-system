using EnduranceJudge.Application.Aggregates.Configurations.Contracts;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.EnduranceEvents;
using EnduranceJudge.Gateways.Persistence.Core;

namespace EnduranceJudge.Gateways.Persistence.Contracts.Queries
{
    public class EnduranceEventQueries : IEnduranceEventQuery, IRepository
    {
        private readonly IState state;
        public EnduranceEventQueries(IState state)
        {
            this.state = state;
        }

        public EnduranceEvent Get()
        {
            return this.state.Event;
        }
    }
}
