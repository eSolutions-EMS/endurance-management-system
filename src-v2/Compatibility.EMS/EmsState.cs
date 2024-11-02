using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using NTS.Compatibility.EMS.Entities.Athletes;
using NTS.Compatibility.EMS.Entities.Countries;
using NTS.Compatibility.EMS.Entities.EnduranceEvents;
using NTS.Compatibility.EMS.Entities.Horses;
using NTS.Compatibility.EMS.Entities.Participants;
using NTS.Compatibility.EMS.Entities.Participations;

namespace NTS.Compatibility.EMS;

public class EmsState
{
    public EmsEnduranceEvent Event { get; set; }
    public List<EmsHorse> Horses { get; private set; } = new();
    public List<EmsAthlete> Athletes { get; private set; } = new();
    public List<EmsParticipant> Participants { get; private set; } = new();
    public List<EmsParticipation> Participations { get; private set; } = new();

    [JsonIgnore]
    public IReadOnlyList<EmsCountry> Countries =>
        new ReadOnlyCollection<EmsCountry>(
            new List<EmsCountry> { new EmsCountry("BGN", "Bulgaria", 1337) }
        );

    public void Set(EmsState initial)
    {
        this.Event = initial.Event;
        this.Horses = initial.Horses;
        this.Athletes = initial.Athletes;
        this.Participants = initial.Participants;
        this.Participations = initial.Participations;
    }
}
