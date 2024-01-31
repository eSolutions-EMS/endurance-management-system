using Common;
using EMS.Domain.Objects;
using EMS.Domain.Setup.Entities;

namespace EMS.Judge.Setup.Events;

public class EventUpdateModel : IIdentifiable, IEventFields
{
    public EventUpdateModel(Event @event)
    {
        this.Id = @event.Id;
        this.Place = @event.Place;
        this.Country = @event.Country;
        this.Staff = @event.Officials.ToList();
        this.Competitions = @event.Competitions.ToList();
    }

    public int Id { get; }
    public string? Place { get; set; }
    public Country? Country { get; set; }
    public List<Official> Staff { get; }
    public List<Competition> Competitions { get; }
}