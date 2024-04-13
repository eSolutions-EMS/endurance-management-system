using NTS.Domain.Setup.Entities;
using Not.Application.Ports.CRUD;
using Not.Exceptions;

namespace NTS.Persistence.Adapters;

public class EventRepository : ParentRepository<Official, Competition, Event, State>, 
    IParentRepository<Official>,
    IParentRepository<Competition>
{

    public EventRepository(IStore<State> store) : base(store)
    {
    }

    public override async Task<Event> Update(Event entity)
    {
        var context = await Store.Load();
        GuardHelper.ThrowIfNull(context.Event);

        foreach (var official in context.Event.Officials)
        {
            entity.Add(official);
        }
        foreach (var competition in context.Event.Competitions)
        {
            entity.Add(competition);
        }
        context.Event = entity;
        await Store.Commit(context);

        return entity;
    }

    public override async Task<Official> Update(int parentId, Official child)
    {
        var context = await Store.Load();
        var parent = context.Event;
        GuardHelper.ThrowIfNull(parent);

        parent.Update(child);
        await Store.Commit(context);

        return child;
    }
    public override async Task<Competition> Update(int parentId, Competition entity)
    {
        var context = await Store.Load();
        var parent = context.Event;
        GuardHelper.ThrowIfNull(parent);

        parent.Update(entity);
        await Store.Commit(context);

        return entity;
    }
}