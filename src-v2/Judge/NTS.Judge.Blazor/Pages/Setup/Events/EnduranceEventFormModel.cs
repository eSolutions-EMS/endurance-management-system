using Not.Blazor.CRUD.Forms.Ports;
using NTS.Domain.Objects;
using NTS.Domain.Setup.Entities;

namespace NTS.Judge.Blazor.Setup.Events;

public class EnduranceEventFormModel : IFormModel<EnduranceEvent>
{
    public EnduranceEventFormModel()
    {
#if DEBUG
        Place = "Каспичан";
        Country = new Country("BG", "zz", "Bulgaria");
#endif
    }

    public int Id { get; private set; }
    public string? Place { get; set; }
    public Country? Country { get; set; }
    public IReadOnlyCollection<Competition> Competitions { get; private set; } = [];
    public IReadOnlyCollection<Official> Officials { get; private set; } = [];

    public void FromEntity(EnduranceEvent enduranceEvent)
    {
        Id = enduranceEvent.Id;
        Place = enduranceEvent.Place;
        Country = enduranceEvent.Country;
        Competitions = enduranceEvent.Competitions;
        Officials = enduranceEvent.Officials;
    }
}
