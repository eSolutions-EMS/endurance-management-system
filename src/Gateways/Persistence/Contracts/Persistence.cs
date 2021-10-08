using EnduranceJudge.Application.Contracts;
using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Domain.Core.Models;

namespace EnduranceJudge.Gateways.Persistence.Contracts
{
    public class Persistence : IPersistence, IService
    {
        private readonly IDataContext context;
        private readonly IStorage storage;

        public Persistence(IDataContext context, IStorage storage)
        {
            this.context = context;
            this.storage = storage;
        }

        public void Update(IAggregateRoot aggregateRoot)
        {
            aggregateRoot.UpdateState(this.context.State);
            this.storage.Snapshot();
        }
    }
}
