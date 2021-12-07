using AutoMapper;
using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Athletes;
using EnduranceJudge.Domain.State.Countries;
using EnduranceJudge.Domain.State.EnduranceEvents;
using EnduranceJudge.Domain.State.Horses;
using EnduranceJudge.Domain.State.Participants;
using Newtonsoft.Json;
using System.Collections.Generic;
using static EnduranceJudge.Localization.Strings.Domain.Countries;

namespace EnduranceJudge.Gateways.Persistence.Contracts
{
    public class State : IState, ISingletonService
    {
        public EnduranceEvent Event { get; set; }
        public List<Horse> Horses { get; } = new();
        public List<Athlete> Athletes { get; } = new();
        public List<Participant> Participants { get; } = new();
        [JsonIgnore]
        public IReadOnlyList<Country> Countries { get; } = new List<Country>()
        {
            new ("BUL", BULGARIA, 1),
        }.AsReadOnly();
    }

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
}
