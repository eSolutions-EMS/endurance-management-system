using Common.Application.CRUD;
using Common.Application.Forms;
using Common.Helpers;
using EMS.Domain.Setup.Entities;

namespace EMS.Judge.Setup.Events;

public class EventManageService : ManageService<Event>, IManageChild<Event, Official>
{
    public EventManageService(IRepository<Event> repository) : base(repository)
    {
    }

    public async Task Add(Official child)
    {
        ThrowHelper.ThrowIfNull(Entity);
        
        Entity.Add(child);
        await Repository.Update(Entity);
    }

    public async Task Remove(Official child)
    {
        ThrowHelper.ThrowIfNull(Entity);

        Entity.Remove(child);
        await Repository.Update(Entity);
    }

    public async Task Update(Official child)
    {
        ThrowHelper.ThrowIfNull(Entity);

        Entity.Update(child);
        await Repository.Update(Entity);
    }
}
