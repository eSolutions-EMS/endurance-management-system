using Common;
using EMS.Domain.Objects;
using EMS.Domain.Setup.Entities;

namespace EMS.Judge.Setup.Events;

public class EventUpdateModel : IIdentifiable
{
    public EventUpdateModel(Event @event)
    {
        this.Id = @event.Id;
        this.Place = @event.Place;
        this.Country = @event.Country;
        this.Staff = @event.Staff.ToList();
        this.Competitions = @event.Competitions.ToList();
    }

    public int Id { get; }
    public string Place { get; set; } = default!;
    public Country Country { get; set; } = default!;
    public List<StaffMember> Staff { get; } = new();
    public List<Competition> Competitions { get; } = new();
}