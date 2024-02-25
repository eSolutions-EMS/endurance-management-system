using Not.Conventions;

namespace EMS.Domain.Core.PersistenceDemo;

public interface IPersistenceDemoRepository : ITransientService
{
    Task<PersistenceDemoModel> Get(Guid id);
}
