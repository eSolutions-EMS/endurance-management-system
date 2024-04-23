using Not.Application.Ports.CRUD;
using Not.Domain;
using Not.Exceptions;

namespace Not.Storage.Adapters.Repositories;

public abstract class BranchRepository<T, TContext> : IRepository<T>
    where T : DomainEntity
    where TContext : class, new()
{
    private readonly IStore<TContext> _store;

    protected BranchRepository(IStore<TContext> store)
    {
        _store = store;
    }

    protected abstract IParent<T>? GetParent(TContext context, int childId);
    protected abstract T? Get(TContext context, int id);

    public virtual async Task<T?> Read(int id)
    {
        var context = await _store.Load();
        return Get(context, id);
    }

    public async Task<T> Update(T entity)
    {
        var context = await _store.Load();
        var parent = GetParent(context, entity.Id);
        GuardHelper.ThrowIfNull(parent);

        parent.Update(entity);
        await _store.Commit(context);

        return entity;
    }

    public virtual Task<IEnumerable<T>> Read(Predicate<T> filter)
    {
        throw NotImplemented();
    }

    public Task<T> Create(T entity)
    {
        throw NotImplemented();
    }
    public Task<T> Delete(int id)
    {
        throw NotImplemented();
    }
    public Task<T> Delete(Predicate<T> filter)
    {
        throw NotImplemented();
    }
    public Task<T> Delete(T entity)
    {
        throw NotImplemented();
    }

    private Exception NotImplemented()
    {
        return new NotImplementedException($"Only 'Read' and 'Update' operations are implemented on '{nameof(BranchRepository<T, TContext>)}'");
    }
}
