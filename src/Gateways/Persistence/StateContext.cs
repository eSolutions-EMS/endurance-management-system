using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Athletes;
using EnduranceJudge.Domain.State.Countries;
using EnduranceJudge.Domain.State.EnduranceEvents;
using EnduranceJudge.Domain.State.Horses;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Domain.State.Participations;
using System.Collections.Generic;

namespace EnduranceJudge.Gateways.Persistence;

public class StateContext : IStateContext
{
    private IState state;

    void IStateContext.Populate(IState state)
    {
        this.state = state;
    }

    public EnduranceEvent Event
    {
        get => this.state?.Event;
        set => this.state.Event = value;
    }
    public List<Horse> Horses => this.state?.Horses;
    public List<Athlete> Athletes => this.state?.Athletes;
    public List<Participant> Participants => this.state?.Participants;
    public List<Participation> Participations => this.state?.Participations;
    public IReadOnlyList<Country> Countries => this.state?.Countries;
}

public interface IStateContext : IState, ISingletonService
{
    internal void Populate(IState state);
}