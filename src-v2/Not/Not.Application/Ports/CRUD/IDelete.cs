using Not.Domain;
using Not.Injection;

namespace Not.Application.Ports.CRUD;

public interface IDelete<T> : ITransientService
    where T : DomainEntity
{
    Task<T> Delete(T entity);
    Task Delete(IEnumerable<T> entities);
}
