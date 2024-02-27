using Not.Domain;
using NTS.Domain.Core.PersistenceDemo;

namespace NTS.Persistence.Adapters;

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
