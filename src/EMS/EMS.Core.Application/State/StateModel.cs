using EnduranceJudge.Application.Services;
using EnduranceJudge.Domain.State.Athletes;
using EnduranceJudge.Domain.State.Countries;
using EnduranceJudge.Domain.State.EnduranceEvents;
using EnduranceJudge.Domain.State.Horses;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Domain.State.Participations;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace EnduranceJudge.Application.State;

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
