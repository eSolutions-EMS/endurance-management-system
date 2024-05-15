using NTS.Domain.Objects;
using NTS.Domain.Setup.Entities;

namespace NTS.Judge.Blazor.Setup.Events;

public class EventFormModel
{
    public EventFormModel()
    {
        //mock default values for testing
        Place = "Каспичан";
        Country = new Country("BG", "Bulgaria");
    }
    public EventFormModel(Event @event)
    {
        Id = @event.Id;
        Place = @event.Place;
        Country = @event.Country;
        Competitions = @event.Competitions;
        Officials = @event.Officials;
    }

    public int? Id { get; }
    public string? Place { get; set; }
    public Country? Country { get; set; }
    public IReadOnlyCollection<Phase>? Phases { get; }
    public IReadOnlyCollection<Competition>? Competitions { get; }
    public IReadOnlyCollection<Official>? Officials { get; }
}