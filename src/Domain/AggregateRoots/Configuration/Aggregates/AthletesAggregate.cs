using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.AggregateRoots.Configuration.Extensions;
using EnduranceJudge.Domain.Core.Exceptions;
using EnduranceJudge.Domain.Core.Extensions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Athletes;
using EnduranceJudge.Localization.Translations;
using static EnduranceJudge.Localization.Translations.Words;
using static EnduranceJudge.Localization.Translations.Messages.DomainValidation;

namespace EnduranceJudge.Domain.AggregateRoots.Configuration.Aggregates
{
    public class AthletesAggregate : IAggregate
    {
        private readonly IState state;
        private readonly Validator<AthleteException> validator;

        internal AthletesAggregate(IState state)
        {
            this.state = state;
            this.validator = new Validator<AthleteException>();
        }

        public Athlete Save(IAthleteState athleteState, int countryId)
        {
            this.state.ValidateThatEventHasNotStarted();

            this.validator.IsRequired(athleteState.FirstName, FIRST_NAME);
            this.validator.IsRequired(athleteState.LastName, LAST_NAME);
            this.validator.IsRequired(athleteState.Category, CATEGORY);
            this.validator.IsRequired(countryId, Entities.COUNTRY);

            var athlete = this.state.Athletes.FindDomain(athleteState.Id);
            if (athlete == null)
            {
                var country = this.state.Countries.FindDomain(countryId);
                athlete = new Athlete(athleteState, country);
                this.state.Athletes.AddOrUpdate(athlete);
            }
            else
            {
                athlete.Category = athleteState.Category;
                athlete.Club = athleteState.Club;
                athlete.FirstName = athlete.FirstName;
                athlete.LastName = athlete.LastName;
                athlete.FeiId = athlete.FeiId;
                if (athlete.Country?.Id != countryId)
                {
                    var country = this.state.Countries.FindDomain(countryId);
                    athlete.Country = country;
                }
                this.UpdateParticipants(athlete);
            }
            return athlete;
        }

        public void Remove(int id)
        {
            this.state.ValidateThatEventHasNotStarted();

            var athlete = this.state.Athletes.FindDomain(id);
            foreach (var participant in this.state.Participants)
            {
                if (participant.Athlete.Equals(athlete))
                {
                    throw Helper.Create<AthleteException>(CANNOT_REMOVE_USED_IN_PARTICIPANT);
                }
            }
            this.state.Athletes.Remove(athlete);
        }

        private void UpdateParticipants(Athlete athlete)
        {
            foreach (var participant in this.state.Participants)
            {
                if (participant.Athlete.Equals(athlete))
                {
                    participant.Athlete.MapFrom(athlete);
                }
            }
        }
    }
}
