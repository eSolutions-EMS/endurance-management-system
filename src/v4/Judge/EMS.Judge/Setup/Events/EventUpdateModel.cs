using Common;
using EMS.Domain.Objects;
using EMS.Domain.Setup.Entities;

namespace EMS.Judge.Setup.Events;

public class EventUpdateModel
{
    public EventUpdateModel()
    {
    }
    public EventUpdateModel(Event @event)
    {
        this.Id = @event.Id;
        this.Place = @event.Place;
        this.Country = @event.Country;
    }

    public int? Id { get; }
    public string? Place { get; set; }
    public Country? Country { get; set; }
}