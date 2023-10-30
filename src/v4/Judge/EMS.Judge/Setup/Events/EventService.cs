using Common.Application.CRUD;
using Common.Conventions;
using Common.Domain.Ports;
using EMS.Domain.Setup.Entities;

namespace EMS.Judge.Setup.Events;

public class EventService : CrudBase<Event, EventCreateModel, EventUpdateModel>, ICreateEvent, IUpdateEvent
{
    public EventService(IRepository<Event> repository) : base(repository)
    {
    }

    protected override Event Build(EventCreateModel model)
    {
        return new Event(model.Place!, model.Country!);
    }
    protected override Event Build(EventUpdateModel model)
    {
        return new Event(model.Place, model.Country);
    }
    protected override EventUpdateModel BuildUpdateModel(Event domainModel)
    {
        return new EventUpdateModel(domainModel);
    }

    public async Task Remove(StaffMember child)
    {
        Entity.Remove(child);
        await Repository.Update(Entity);

        UpdateModel!.Staff.Remove(child);
    }
    public async Task Add(StaffMember child)
    {
        Entity.Add(child);
        await Repository.Update(Entity);

        UpdateModel!.Staff.Add(child);
    }
}

public interface ICreateEvent : ICreate<EventCreateModel>, ISingletonService
{
}
public interface IUpdateEvent : IUpdate<EventUpdateModel>, IRead<Event>, IParent<StaffMember>
{
}