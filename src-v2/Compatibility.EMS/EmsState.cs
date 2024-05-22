using NTS.Compatibility.EMS.Entities.Athletes;
using NTS.Compatibility.EMS.Entities.Countries;
using NTS.Compatibility.EMS.Entities.EnduranceEvents;
using NTS.Compatibility.EMS.Entities.Horses;
using NTS.Compatibility.EMS.Entities.Participants;
using NTS.Compatibility.EMS.Entities.Participations;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace NTS.Compatibility.EMS;

public class EmsState
{
    public EnduranceEvent Event { get; set; }
    public List<Horse> Horses { get; private set; } = new();
    public List<Athlete> Athletes { get; private set; } = new();
    public List<Participant> Participants { get; private set; } = new();
    public List<Participation> Participations { get; private set; } = new();
    [JsonIgnore]
    public IReadOnlyList<Country> Countries
        => new ReadOnlyCollection<Country>(new List<Country> { new Country("BGN", "Bulgaria", 1337) });

    public void Set(EmsState initial)
    {
        this.Event = initial.Event;
        this.Horses = initial.Horses;
        this.Athletes = initial.Athletes;
        this.Participants = initial.Participants;
        this.Participations = initial.Participations;
    }
}
