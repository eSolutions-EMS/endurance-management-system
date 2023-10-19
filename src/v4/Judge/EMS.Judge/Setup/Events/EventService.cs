using Common.Conventions;
using EMS.Domain.Ports;
using EMS.Domain.Setup.Entities;

namespace EMS.Judge.Setup.Events;

public class EventService : IEventCreateService, IEventUpdateService
{
    private readonly IRepository<Event> repository;

    public EventService(IRepository<Event> repository)
    {
        this.repository = repository;
    }

    public EventCreateModel CreateModel { get; private set; } = new();
    public EventUpdateModel? UpdateModel { get; private set; }

    public async Task Create()
    {
        var @event = new Event(this.CreateModel.Place!, this.CreateModel.Country!);
        await repository.Create(@event);
        this.UpdateModel = new EventUpdateModel(@event);
        this.CreateModel = new();
    }

    public async Task Read()
    {
        var @event = await repository.Read(0);
        if (@event is null)
        {
            return;
        }
        this.UpdateModel = new EventUpdateModel(@event);
    }

    public async Task Update()
    {
        var @event = new Event(this.UpdateModel!.Place, this.UpdateModel!.Country);
        await this.repository.Update(@event);
    }

    public async Task Delete()
    {
        if (this.UpdateModel is null)
        {
            return;
        }
        await repository.Delete(this.UpdateModel.Id);
        this.UpdateModel = null;
    }
}

public interface IEventCreateService : ITransientService
{
    EventCreateModel CreateModel { get; }
    Task Create();
}

public interface IEventUpdateService : ITransientService
{
    EventUpdateModel? UpdateModel { get; }
    Task Read();
    Task Update();
    Task Delete();
}
