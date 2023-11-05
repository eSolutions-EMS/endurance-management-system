using Common.Application.CRUD;
using Common.Conventions;
using Common.Domain;

namespace Common.Application.Forms;

public abstract class ManageService<T> : IManage<T>
    where T : DomainEntity
{
    public ManageService(IRepository<T> repository)
    {
        Repository = repository;
    }

    protected IRepository<T> Repository { get; }

    public T? Entity { get; private set; }

    public async Task<T?> Read(int id)
    {
        var entity = await Repository.Read(id);
        if (entity == null)
        {
            return null;
        }
        Entity = entity;
        return entity;
    }

    public async Task Update(T entity)
    {
        await Repository.Update(entity);
    }
}

public interface IManage<T> : ISingletonService
    where T : DomainEntity
{
    T? Entity { get; }
    Task<T?> Read(int id);
    Task Update(T entity);
}
