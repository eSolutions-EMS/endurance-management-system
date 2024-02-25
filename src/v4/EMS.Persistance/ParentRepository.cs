using Not.Domain;
using Not.Helpers;

namespace EMS.Persistence;

public abstract class ParentRepository<T, TParent, TDataContext> : RepositoryBase<TParent, TDataContext>
    where TParent : DomainEntity, IParent<T>
    where T : DomainEntity
    where TDataContext : class, IEntityContext<TParent>, new()
{
    public ParentRepository(IStore<TDataContext> store) : base(store)
    {
    }

    public async Task<T> Create(int parentId, T child)
    {
        return await InternalCreate(x => x.Entities.Find(x => x.Id == parentId), child);
    }

    public async Task<T> Delete(int parentId, T child)
    {
        return await InternalDelete(x => x.Entities.Find(x => x.Id == parentId), child);
    }

    protected async Task<TInner> InternalCreate<TInner, TInnerParent>(Func<TDataContext, TInnerParent?> parentGetter, TInner child)
        where TInner : DomainEntity
        where TInnerParent : IParent<TInner>
    {
        var context = await Store.Load();
        var parent = parentGetter(context);
        ThrowHelper.ThrowIfNull(parent);

        parent.Add(child);
        await Store.Commit(context);

        return child;
    }

    protected async Task<TInner> InternalDelete<TInner, TInnerParent>(Func<TDataContext, TInnerParent?> parentGetter, TInner child)
        where TInner : DomainEntity
        where TInnerParent : IParent<TInner>
    {
        var context = await Store.Load();
        var parent = parentGetter(context);
        ThrowHelper.ThrowIfNull(parent);

        parent.Remove(child);
        await Store.Commit(context);

        return child;
    }
}

public abstract class ParentRepository<T1, T2, TParent, TDataContext> : ParentRepository<T1, TParent, TDataContext>
    where TParent : DomainEntity, IParent<T1>, IParent<T2>
    where T1 : DomainEntity
    where T2 : DomainEntity
    where TDataContext : class, IEntityContext<TParent>, new()
{
    protected ParentRepository(IStore<TDataContext> store) : base(store)
    {
    }

    public async Task<T2> Create(int parentId, T2 child)
    {
        return await InternalCreate(x => x.Entities.Find(x => x.Id == parentId), child);
    }

    public async Task<T2> Delete(int parentId, T2 child)
    {
        return await InternalDelete(x => x.Entities.Find(x => x.Id == parentId), child);
    }
}
