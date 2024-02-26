using EMS.Domain.Objects;
using EMS.Domain.Setup.Entities;

namespace EMS.Judge.UI.Setup.Events;

public class EventFieldsModel
{
    public EventFieldsModel()
    {
    }
    public EventFieldsModel(Event @event)
    {
        this.Id = @event.Id;
        this.Place = @event.Place;
        this.Country = @event.Country;
    }

    public int? Id { get; }
    public string? Place { get; set; }
    public Country? Country { get; set; }
}