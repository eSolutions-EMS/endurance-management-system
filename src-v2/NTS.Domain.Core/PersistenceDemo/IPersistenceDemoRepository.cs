using Not.Injection;

namespace NTS.Domain.Core.PersistenceDemo;

public interface IPersistenceDemoRepository : ITransientService
{
    Task<PersistenceDemoModel> Get(Guid id);
}
