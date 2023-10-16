using Common.Domain;
using EMS.Domain.Core.PersistenceDemo;

namespace EMS.Persistence.Adapters;

public class DemoEventRepository : IPersistenceDemoRepository
{
    public Task<PersistenceDemoModel> Get(Guid id)
    {
        return Task.FromResult(new PersistenceDemoModel { Id = id } );
    }

    public Task ResetState()
    {
        return Task.CompletedTask;
    }
}
