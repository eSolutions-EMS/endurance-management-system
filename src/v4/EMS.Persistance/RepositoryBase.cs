using Not.Application.Ports.CRUD;
using Not.Domain;
using Not.Helpers;

namespace EMS.Persistence;

public abstract class RepositoryBase<T, TDataContext> : IRepository<T>
    where TDataContext : class, IEntityContext<T>, new()
    where T : DomainEntity
{

    public RepositoryBase(IStore<TDataContext> store)
    {
        this.Store = store;
    }

    protected IStore<TDataContext> Store { get; }

    public virtual async Task<T> Create(T entity)
    {
        var context = await Store.Load();
        context.Entities.Add(entity);
        await Store.Commit(context);

        return entity;
    }

    public virtual async Task<T> Delete(T entity)
    {
        var context = await Store.Load();
        context.Entities.Remove(entity);
        await Store.Commit(context);

        return entity;
    }

    public virtual async Task<T> Delete(int id)
    {
        var context = await Store.Load();
        var existing = context.Entities.FirstOrDefault(x => x.Id == id);
        ThrowHelper.ThrowIfNull(existing);

        context.Entities.Remove(existing);
        await Store.Commit(context);

        return existing;
    }

    public virtual async Task<T> Delete(Predicate<T> filter)
    {
        var context = await Store.Load();
        var existing = context.Entities.FirstOrDefault(x => filter(x));
        ThrowHelper.ThrowIfNull(existing);

        context.Entities.Remove(existing);
        await Store.Commit(context);
        
        return existing;
    }

    public virtual async Task<T?> Read(int id)
    {
        var context = await Store.Load();
        return context.Entities.FirstOrDefault(x => x.Id == id);
    }

    public virtual async Task<IEnumerable<T>> Read(Predicate<T> filter)
    {
        var context = await Store.Load();
        return context.Entities.Where(x => filter(x));
    }

    // TODO: fix docs
    /// <summary>
    /// This method is executed during Update to preseve the current children of a given entity. Leave empty if entity has no children.
    /// <seealso href="https://github.com/eSolutions-EMS/endurance-management-system/wiki/DomainRepository">Details</seealso>
    /// </summary>
    /// <param name="entity"></param>
    public abstract Task<T> Update(T entity);
}
