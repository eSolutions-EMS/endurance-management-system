using AutoMapper;
using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Core.Mappings;
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
    public List<Horse> Horses { get; } = new();
    public List<Athlete> Athletes { get; } = new();
    public List<Participant> Participants { get; } = new();
    public List<Participation> Participations { get; } = new();

    [JsonIgnore]
    public IReadOnlyList<Country> Countries
        => PersistenceConstants.Countries.List.AsReadOnly();
}

// TODO: remove
public class StateMaps : ICustomMapConfiguration
{
    public void AddFromMaps(IProfileExpression profile)
    {
        profile.CreateMap<State, State>()
            .ForMember(x => x.Countries, opt => opt.Ignore());
    }

    public void AddToMaps(IProfileExpression profile)
    {
    }
}