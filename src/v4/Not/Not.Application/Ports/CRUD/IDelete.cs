using Not.Conventions;
using Not.Domain;

namespace Not.Application.Ports.CRUD;

public interface IDelete<T> : ITransientService
    where T : DomainEntity
{
    Task<T> Delete(T entity);
}
