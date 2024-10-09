using AngleSharp.Dom;
using Not.Application.Ports.CRUD;
using Not.Blazor.Ports.Behinds;
using Not.Domain;
using Not.Events;
using Not.Exceptions;
namespace Not.Application.Contexts;

public abstract class BehindContext<T> : IBehindContext<T>
    where T : DomainEntity
{
    protected BehindContext(IRepository<T> entities)
    {
        Repository = entities;
    }
    protected IRepository<T> Repository { get; }

    public T? Entity { get; set; }

    public EventManager Loaded { get; } = new();

    public bool HasLoaded()
    {
        return Entity != null;
    }

    public async Task Update()
    {
        GuardHelper.ThrowIfDefault(Entity);
        await Repository.Update(Entity);
    }
}
