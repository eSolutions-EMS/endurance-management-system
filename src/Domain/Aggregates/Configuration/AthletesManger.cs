using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Aggregates.Configuration.Extensions;
using EnduranceJudge.Domain.Core.Exceptions;
using EnduranceJudge.Domain.Core.Extensions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Athletes;
using EnduranceJudge.Localization.Translations;
using static EnduranceJudge.Localization.Translations.Messages;

namespace EnduranceJudge.Domain.Aggregates.Configuration
{
    public class AthletesManger : ManagerObjectBase
    {
        private readonly IState state;

        internal AthletesManger(IState state)
        {
            this.state = state;
        }

        public Athlete Save(IAthleteState athleteState, int countryId)
        {
            this.state.ValidateThatEventHasNotStarted();

            this.Validate<AthleteException>(() =>
            {
                athleteState.FirstName.IsRequired(Words.FIRST_NAME);
                athleteState.LastName.IsRequired(Words.LAST_NAME);
                athleteState.Category.IsRequired(Words.CATEGORY);
                countryId.IsRequired(Entities.COUNTRY);
            });

            var athlete = this.state.Athletes.FindDomain(athleteState.Id);
            if (athlete == null)
            {
                var country = this.state.Countries.FindDomain(countryId);
                athlete = new Athlete(athleteState, country);
                this.state.Athletes.AddOrUpdate(athlete);
                this.UpdateParticipants(athlete);
            }
            else
            {
                athlete.Category = athleteState.Category;
                athlete.Club = athleteState.Club;
                athlete.FirstName = athlete.FirstName;
                athlete.LastName = athlete.LastName;
                athlete.FeiId = athlete.FeiId;
                if (athlete.Country.Id != countryId)
                {
                    var country = this.state.Countries.FindDomain(countryId);
                    athlete.Country = country;
                }
            }
            return athlete;
        }

        public void Remove(int id)
        {
            this.state.ValidateThatEventHasNotStarted();

            var athlete = this.state.Athletes.FindDomain(id);
            this.Validate<AthleteException>(() =>
            {
                foreach (var participant in this.state.Participants)
                {
                    if (participant.Athlete.Equals(athlete))
                    {
                        throw new DomainException(CANNOT_REMOVE_USED_IN_PARTICIPANT);
                    }
                }
            });

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
