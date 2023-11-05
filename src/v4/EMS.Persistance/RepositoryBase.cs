using Common.Application.CRUD;
using Common.Domain;

namespace EMS.Persistence;

public abstract class RepositoryBase<T> : IRepository<T>
    where T : DomainEntity
{
    public virtual Task<T> Create(T entity)
    {
        throw new NotImplementedException();
    }

    public virtual Task Delete(T entity)
    {
        throw new NotImplementedException();
    }

    public virtual Task Delete(int id)
    {
        throw new NotImplementedException();
    }

    public virtual Task<int> Delete(Predicate<T> filter)
    {
        throw new NotImplementedException();
    }

    public virtual Task<T?> Read(int id)
    {
        throw new NotImplementedException();
    }

    public virtual Task<IEnumerable<T>> Read(Predicate<T> filter)
    {
        throw new NotImplementedException();
    }

    public virtual Task<T> Update(T entity)
    {
        throw new NotImplementedException();
    }
}
