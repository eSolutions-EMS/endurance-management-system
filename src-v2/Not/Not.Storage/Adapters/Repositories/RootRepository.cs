﻿using Not.Storage.Ports.States;

namespace Not.Storage.Adapters.Repositories;

/// <summary>
/// Represent a set of operations for root-level entiries in a Tree-like data structure.
/// Implements IReposistory to allow for streamline API, but does not support any Delete operations
/// or Read(Predicate) operations as they are not necessary.
/// </summary>
/// <typeparam name="T">Type of the Root entity</typeparam>
/// <typeparam name="TState">Type of the state object containing the Root entity</typeparam>
public abstract class RootRepository<T, TState> : IRepository<T>
    where T : DomainEntity
    where TState : class, ITreeState<T>, new()
{
    readonly IStore<TState> _store;

    public RootRepository(IStore<TState> store)
    {
        _store = store;
    }

    public async Task Create(T entity)
    {
        var state = await _store.Transact();
        state.Root = entity;
        await _store.Commit(state);
    }

    public async Task<T?> Read(Predicate<T> _)
    {
        return await Read(0);
    }

    public async Task<T?> Read(int _)
    {
        var state = await _store.Readonly();
        return state.Root;
    }

    public async Task Update(T entity)
    {
        await Create(entity);
    }

    public Task Delete(int id)
    {
        throw NotImplemented();
    }

    public Task Delete(Predicate<T> filter)
    {
        throw NotImplemented();
    }

    public Task Delete(T entity)
    {
        throw NotImplemented();
    }

    public Task Delete(IEnumerable<T> entities)
    {
        throw NotImplemented();
    }

    public Task<IEnumerable<T>> ReadAll()
    {
        throw NotImplemented();
    }

    public Task<IEnumerable<T>> ReadAll(Predicate<T> filter)
    {
        throw NotImplemented();
    }

    Exception NotImplemented()
    {
        return new Exception(
            "Only Create, Read and Update operations are implemented for Root entities."
        );
    }
}
