using Not.Helpers;
using EMS.Domain.Setup.Entities;
using Not.Application.Ports.CRUD;

namespace EMS.Persistence.Adapters;

public class EventRepository : ParentRepository<Official, Event, State>, IParentRepository<Official>
{

    public EventRepository(IStore<State> store) : base(store)
    {
    }

    public override async Task<Event> Update(Event entity)
    {
        var context = await Store.Load();
        ThrowHelper.ThrowIfNull(context.Event);

        foreach (var official in context.Event.Officials)
        {
            entity.Add(official);
        }
        context.Event = entity;
        await Store.Commit(context);

        return entity;
    }

    public async Task<Official> Update(Official child)
    {
        var context = await Store.Load();
        var existing = context.Officials.Find(x => x == child);
        ThrowHelper.ThrowIfNull(existing);

        context.Event.Update(child);
        await Store.Commit(context);

        return child;
    }
}