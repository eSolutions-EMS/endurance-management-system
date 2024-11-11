using Not.Application.Ports.CRUD;
using Not.Domain;
using Not.Exceptions;

namespace Not.Application.Contexts;

public abstract class BehindContext<T>
    where T : DomainEntity
{
    protected BehindContext(IRepository<T> repository)
    {
        Repository = repository;
    }

    protected IRepository<T> Repository { get; }
    public T? Entity { get; set; }

    public bool HasLoaded()
    {
        return Entity != null;
    }

    public async Task Persist()
    {
        GuardHelper.ThrowIfDefault(Entity);
        await Repository.Update(Entity);
    }
}
