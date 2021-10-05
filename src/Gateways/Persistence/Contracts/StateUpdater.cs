using EnduranceJudge.Application.Contracts;
using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Gateways.Persistence;
using System.Threading.Tasks;

namespace EnduranceJudge.Gateways.Persistence.Contracts
{
    public class StateUpdater : IStateUpdater, IService
    {
        private readonly IDataContext context;
        private readonly IPersistence persistence;

        public StateUpdater(IDataContext context, IPersistence persistence)
        {
            this.context = context;
            this.persistence = persistence;
        }

        public async Task Update(IAggregateRoot aggregateRoot)
        {
            aggregateRoot.UpdateState(this.context.State);
            await this.persistence.Snapshot();
        }
    }
}
