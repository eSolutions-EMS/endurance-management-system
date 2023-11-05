using Common.Application.Forms;
using Common.Domain.Ports;
using Common.Helpers;
using EMS.Domain.Setup.Entities;

namespace EMS.Judge.Setup.Events;

public class EventManageService : ManageService<Event>, IManageChild<Event, StaffMember>
{
    public EventManageService(IRepository<Event> repository) : base(repository)
    {
    }

    public async Task Add(StaffMember child)
    {
        ThrowHelper.ThrowIfNull(Entity);
        
        Entity.Add(child);
        await Repository.Update(Entity);
    }

    public async Task Remove(StaffMember child)
    {
        ThrowHelper.ThrowIfNull(Entity);

        Entity.Remove(child);
        await Repository.Update(Entity);
    }

    public async Task Update(StaffMember child)
    {
        ThrowHelper.ThrowIfNull(Entity);

        Entity.Update(child);
        await Repository.Update(Entity);
    }
}
