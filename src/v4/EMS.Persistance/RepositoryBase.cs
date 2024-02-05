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

    /// <summary>
    /// This method is executed during Update to preseve the current children of a given entity. Leave empty if entity has no children.
    /// <seealso href="https://github.com/eSolutions-EMS/endurance-management-system/wiki/DomainRepository">Details</seealso>
    /// </summary>
    /// <param name="entity"></param>
    protected abstract void PreserveChildrenDuringUpdate(T existing, T update);
}
