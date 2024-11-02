using Not.Domain;
using Not.Injection;

namespace Not.Application.Ports.CRUD;

public interface IUpdate<T> : ITransientService
    where T : DomainEntity
{
    Task<T> Update(T entity);
}
