using Core.Domain.State.Athletes;
using Core.Domain.State.Countries;
using Core.Domain.State.EnduranceEvents;
using Core.Domain.State.Horses;
using Core.Domain.State.Participants;
using Core.Domain.State.Participations;
using EMS.Judge.Application.Services;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace EMS.Judge.Application.State;

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
