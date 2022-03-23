using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Athletes;
using EnduranceJudge.Domain.State.Countries;
using EnduranceJudge.Domain.State.EnduranceEvents;
using EnduranceJudge.Domain.State.Horses;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Domain.State.Participations;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace EnduranceJudge.Gateways.Persistence.Contracts;

public class State : IState
{
    public EnduranceEvent Event { get; set; }
    public List<Horse> Horses { get; private set; } = new();
    public List<Athlete> Athletes { get; private set; } = new();
    public List<Participant> Participants { get; private set; } = new();
    public List<Participation> Participations { get; private set; } = new();

    [JsonIgnore]
    public IReadOnlyList<Country> Countries
        => PersistenceConstants.Countries.List.AsReadOnly();

    internal void Restore(IState state)
    {
        this.Event = state.Event;
        this.Horses = state.Horses;
        this.Athletes = state.Athletes;
        this.Participants = state.Participants;
        this.Participations = state.Participations;
    }
}
