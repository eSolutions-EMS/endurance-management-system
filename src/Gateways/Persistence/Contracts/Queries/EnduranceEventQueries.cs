using EnduranceJudge.Application.Aggregates.Configurations.Contracts;
using EnduranceJudge.Domain.State.EnduranceEvents;
using EnduranceJudge.Gateways.Persistence.Core;

namespace EnduranceJudge.Gateways.Persistence.Contracts.Queries
{
    public class EnduranceEventQueries : IEnduranceEventQuery, IRepository

    {
        private readonly IDataContext context;
        public EnduranceEventQueries(IDataContext context)
        {
            this.context = context;
        }

        public EnduranceEvent Get()
        {
            return this.context.State.Event;
        }
    }
}
