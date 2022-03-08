using EnduranceJudge.Core.Utilities;
using EnduranceJudge.Domain.AggregateRoots.Configuration.Aggregates;
using EnduranceJudge.Domain.Core.Extensions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Validation;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.EnduranceEvents;
using EnduranceJudge.Domain.State.Personnels;
using EnduranceJudge.Localization.Strings;
using static EnduranceJudge.Localization.Strings.Words;

namespace EnduranceJudge.Domain.AggregateRoots.Configuration
{
    public class ConfigurationRoot : IAggregateRoot
    {
        private readonly IState state;
        private readonly Validator<EnduranceEventException> validator;

        public ConfigurationRoot()
        {
            this.state = StaticProvider.GetService<IState>();
            this.Competitions = new CompetitionsAggregate(this.state);
            this.Phases = new PhasesAggregate(this.state);
            this.Athletes = new AthletesAggregate(this.state);
            this.Horses = new HorsesAggregate(this.state);
            this.Participants = new ParticipantsAggregate(this.state);
            this.validator = new Validator<EnduranceEventException>();
        }

        public EnduranceEvent Update(string name, int countryId, string populatedPlace)
        {
            this.validator.IsRequired(name, NAME);
            this.validator.IsRequired(populatedPlace, POPULATED_PLACE);
            this.validator.IsRequired(countryId, Entities.COUNTRY_ENTITY);

            var country = this.state.Countries.FindDomain(countryId);
            if (this.state.Event == null)
            {
                this.state.Event = new EnduranceEvent(name, country)
                {
                    PopulatedPlace = populatedPlace,
                };
            }
            else
            {
                this.state.Event.Name = name;
                this.state.Event.PopulatedPlace = populatedPlace;
                this.state.Event.Country = country;
            }
            return this.state.Event;
        }

        public Personnel Save(IPersonnelState state)
        {
            var personnel = new Personnel(state);
            this.state.Event.Save(personnel);
            return personnel;
        }

        public CompetitionsAggregate Competitions { get; }
        public PhasesAggregate Phases { get; }
        public AthletesAggregate Athletes { get; }
        public HorsesAggregate Horses { get; }
        public ParticipantsAggregate Participants { get; }

        public void __REVERT_START_PARTICIPATIONS__()
        {
            this.state.Event.HasStarted = false;
            this.Participants.__REVERT_START_PARTICIPATIONS__();
        }
    }
}
