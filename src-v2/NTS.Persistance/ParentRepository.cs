using Not.Application.Ports.CRUD;
using Not.Domain;
using Not.Exceptions;

namespace NTS.Persistence;

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

    public abstract Task<T> Update(int parentId, T child);

    public override Task<TParent> Update(TParent entity)
    {
        throw new NotImplementedException(
            $"If '{typeof(TParent).Name}' is root entity (i.e entity that sits in the root of the database state)" +
            $" you have to override this method and implement its details, including child preservation. " +
            $"If this is a child entity, then you need to update it using '{nameof(IParentRepository<TParent>)}' instead");
    }

    protected async Task<TInner> InternalCreate<TInner, TInnerParent>(Func<TDataContext, TInnerParent?> parentGetter, TInner child)
        where TInner : DomainEntity
        where TInnerParent : IParent<TInner>
    {
        var context = await Store.Load();
        var parent = parentGetter(context);
        GuardHelper.ThrowIfNull(parent);

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
        GuardHelper.ThrowIfNull(parent);

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

    public abstract Task<T2> Update(int parentId, T2 child);
}
