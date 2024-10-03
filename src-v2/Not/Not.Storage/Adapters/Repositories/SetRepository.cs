using Not.Storage.Ports.States;

namespace Not.Storage.Adapters.Repositories;

public abstract class SetRepository<T, TState> : IRepository<T>
    where T : DomainEntity
    where TState : class, ISetState<T>, new()
{
    public SetRepository(IStore<TState> store)
    {
        Store = store;
    }

    protected IStore<TState> Store { get; }

    public virtual async Task<T> Create(T entity)
    {
        var state = await Store.Transact();
        state.EntitySet.Add(entity);
        await Store.Commit(state);
        return entity;
    }


    public async Task<T> Delete(int id)
    {
        var state = await Store.Transact();
        var match = state.EntitySet.FirstOrDefault(x => x.Id == id);
        if (match == null)
        {
            return default;
        }
        state.EntitySet.Remove(match);
        await Store.Commit(state);
        return match;
    }

    public async Task<T> Delete(Predicate<T> filter)
    {
        var state = await Store.Transact();
        state.EntitySet.RemoveAll(filter);
        await Store.Commit(state);
        return default!;
    }

    public virtual async Task<T> Delete(T entity)
    {
        var state = await Store.Transact();
        state.EntitySet.Remove(entity);
        await Store.Commit(state);
        return entity;
    }

    public async Task Delete(IEnumerable<T> entities)
    {
        var state = await Store.Transact();
        foreach (var entity in entities)
        {
            state.EntitySet.Remove(entity);
        }
        await Store.Commit(state);
    }

    public async Task<T?> Read(int id)
    {
        var state = await Store.Readonly();
        return state.EntitySet.FirstOrDefault(x => x.Id == id);
    }

    public async Task<IEnumerable<T>> ReadAll()
    {
        var state = await Store.Readonly();
        return state.EntitySet;
    }

    public async Task<IEnumerable<T>> ReadAll(Predicate<T> filter)
    {
        var state = await Store.Readonly();
        return state.EntitySet.Where(x => filter(x));
    }

    public async Task<T?> Read(Predicate<T> filter)
    {
        var state = await Store.Readonly();
        return state.EntitySet.FirstOrDefault(x => filter(x));
    }

    public virtual async Task<T> Update(T entity)
    {
        var state = await Store.Transact();

        PerformUpdate(state, entity);

        await Store.Commit(state);
        return entity;
    }

    protected virtual void PerformUpdate(TState state, T entity)
    {
        var index = state.EntitySet.IndexOf(entity);
        state.EntitySet.Remove(entity);
        state.EntitySet.Insert(index, entity);
    }
}
