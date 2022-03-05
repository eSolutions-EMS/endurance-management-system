using EnduranceJudge.Core.Utilities;
using EnduranceJudge.Domain.Core.Extensions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.EnduranceEvents;
using EnduranceJudge.Domain.State.Personnels;
using EnduranceJudge.Localization.Translations;
using static EnduranceJudge.Localization.Translations.Words;

namespace EnduranceJudge.Domain.Aggregates.Configuration
{
    public class ConfigurationManager : IAggregate, IAggregateRoot
    {
        private readonly IState state;
        private readonly Validator<EnduranceEventException> validator;

        public ConfigurationManager()
        {
            this.state = StaticProvider.GetService<IState>();
            this.Competitions = new CompetitionsManager(this.state);
            this.Phases = new PhasesManager(this.state);
            this.Athletes = new AthletesManager(this.state);
            this.Horses = new HorsesManager(this.state);
            this.Participants = new ParticipantsManager(this.state);
            this.validator = new Validator<EnduranceEventException>();
        }

        public EnduranceEvent Update(string name, int countryId, string populatedPlace)
        {
            this.validator.IsRequired(name, NAME);
            this.validator.IsRequired(populatedPlace, POPULATED_PLACE);
            this.validator.IsRequired(countryId, Entities.COUNTRY);

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

        public CompetitionsManager Competitions { get; }
        public PhasesManager Phases { get; }
        public AthletesManager Athletes { get; }
        public HorsesManager Horses { get; }
        public ParticipantsManager Participants { get; }

        public void __REVERT_START_PARTICIPATIONS__()
        {
            this.state.Event.HasStarted = false;
            this.Participants.__REVERT_START_PARTICIPATIONS__();
        }
    }
}
