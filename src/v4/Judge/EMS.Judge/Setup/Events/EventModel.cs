using Common.Domain;
using EMS.Domain.Objects;
using EMS.Domain.Setup.Entities;
using EMS.Judge.Setup.Competitions;
using EMS.Judge.Setup.Staff;
using System.ComponentModel.DataAnnotations;

namespace EMS.Judge.Setup.Events;

public class EventCreateModel
{
    [Required]
    public string? Place { get; set; }
    [Required]
    public Country? Country { get; set; }
}

public class EventUpdateModel : IEntity
{
    public EventUpdateModel(Event @event)
    {
        this.Id = @event.Id;
        this.Place = @event.Place;
        this.Country = @event.Country;
        this.Staff = @event.Staff.Select(x => new StaffMemberCreateModel(x)).ToList();
        this.Competitions = @event.Competitions.Select(x => new CompetitionCreateModel(x)).ToList();
    }

    public int Id { get; }
    public string Place { get; set; } = default!;
    public Country Country { get; set; } = default!;
    public List<StaffMemberCreateModel> Staff { get; } = new();
    public List<CompetitionCreateModel> Competitions { get; } = new();
}