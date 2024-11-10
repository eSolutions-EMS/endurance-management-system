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
    public List<EmsHorse> Horses { get; private set; } = [];
    public List<EmsAthlete> Athletes { get; private set; } = [];
    public List<EmsParticipant> Participants { get; private set; } = [];
    public List<EmsParticipation> Participations { get; private set; } = [];

    [JsonIgnore]
    public IReadOnlyList<EmsCountry> Countries =>
        new ReadOnlyCollection<EmsCountry>([new("BGN", "Bulgaria", 1337)]);

    public void Set(EmsState initial)
    {
        Event = initial.Event;
        Horses = initial.Horses;
        Athletes = initial.Athletes;
        Participants = initial.Participants;
        Participations = initial.Participations;
    }
}
