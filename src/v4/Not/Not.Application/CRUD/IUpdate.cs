using Not.Conventions;
using Not.Domain;

namespace Not.Application.CRUD;

public interface IUpdate<T> : ITransientService
    where T : DomainEntity
{
    Task<T> Update(T entity); 
}
