using Common.Conventions;
using Common.Domain;
using Common.Domain.Ports;

namespace Common.Application.Forms;

public abstract class UpdateFormService<T> : IUpdateForm<T>
    where T : DomainEntity
{
    public UpdateFormService(IRepository<T> repository)
    {
        Repository = repository;
    }

    protected IRepository<T> Repository { get; }

    public T? Entity { get; private set; }

    public async Task<T> Read(int id)
    {
        var entity = await Repository.Read(id);
        if (entity == null)
        {
            // TODO: toast + navigate back;
            throw new NotImplementedException($"'{nameof(Entity)}' null behavior not defined yet");
        }
        Entity = entity;
        return entity;
    }

    public async Task Update(T entity)
    {
        await Repository.Update(entity);
    }
}

public interface IUpdateForm<T> : ISingletonService
    where T : DomainEntity
{
    T? Entity { get; }
    Task<T> Read(int id);
    Task Update(T entity);
}
