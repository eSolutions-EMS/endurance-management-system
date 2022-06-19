using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Athletes;
using EnduranceJudge.Domain.State.Countries;
using EnduranceJudge.Domain.State.EnduranceEvents;
using EnduranceJudge.Domain.State.Horses;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Domain.State.Participations;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace EnduranceJudge.Application;

public class State : IState
{
    public EnduranceEvent Event { get; set; }
    public List<Horse> Horses { get; set; } = new();
    public List<Athlete> Athletes { get; set; } = new();
    public List<Participant> Participants { get; set; } = new();
    public List<Participation> Participations { get; set; } = new();

    [JsonIgnore]
    public IReadOnlyList<Country> Countries
        => ApplicationConstants.Countries.List.AsReadOnly();
}
