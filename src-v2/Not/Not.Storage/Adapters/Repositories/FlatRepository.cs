namespace Not.Storage.Adapters.Repositories;

public abstract class FlatRepository<T, TState> : IRepository<T>
    where T : DomainEntity
    where TState : class, IFlatState<T> , new()
{
    private readonly IStore<TState> _store;

    public FlatRepository(IStore<TState> store)
    {
        _store = store;
    }

    public async Task<T> Create(T entity)
    {
        var state = await _store.Load();
        state.Entity = entity;
        await _store.Commit(state);
        return entity;
    }

    public async Task<T> Delete(int id)
    {
        var state = await _store.Load();
        if (state.Entity == null || state.Entity.Id != id)
        {
            return default;
        }
        state.Entity = null;
        await _store.Commit(state);
        return default;
    }

    public Task<T> Delete(Predicate<T> filter)
    {
        throw new NotImplementedException("Doesnt make sense");
    }

    public async Task<T> Delete(T entity)   
    {
        var state = await _store.Load();
        if (state.Entity != entity)
        {
            return default;
        }
        state.Entity = null;
        await _store.Commit(state);
        return entity;
    }

    public Task<IEnumerable<T>> Read(Predicate<T> filter)
    {
        throw new NotImplementedException("Doesnt make sense");
    }

    public async Task<T?> Read(int id)
    {
        var state = await _store.Load();
        return state.Entity?.Id == id
            ? state.Entity
            : null;
    }

    public async Task<T> Update(T entity)
    {
        return await Create(entity);
    }
}
