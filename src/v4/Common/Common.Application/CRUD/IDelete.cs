using Common.Conventions;
using Common.Domain;

namespace Common.Application.CRUD;

public interface IDelete<in T> : ITransientService
    where T : DomainEntity
{
    Task Delete(T entity);
}
