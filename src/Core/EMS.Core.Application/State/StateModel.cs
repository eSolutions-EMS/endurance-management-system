using EMS.Core.Application.Services;
using EMS.Core.Domain.State.Athletes;
using EMS.Core.Domain.State.Countries;
using EMS.Core.Domain.State.EnduranceEvents;
using EMS.Core.Domain.State.Horses;
using EMS.Core.Domain.State.Participants;
using EMS.Core.Domain.State.Participations;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace EMS.Core.Application.State;

public class StateModel : IStateSetter
{
    public EnduranceEvent Event { get; set; }
    public List<Horse> Horses { get; private set; } = new();
    public List<Athlete> Athletes { get; private set; } = new();
    public List<Participant> Participants { get; private set; } = new();
    public List<Participation> Participations { get; private set; } = new();
    [JsonIgnore]
    public IReadOnlyList<Country> Countries
        => ApplicationConstants.Countries.List.AsReadOnly();
    void IStateSetter.Set(StateModel initial)
    {
        this.Event = initial.Event;
        this.Horses = initial.Horses;
        this.Athletes = initial.Athletes;
        this.Participants = initial.Participants;
        this.Participations = initial.Participations;
    }
}
