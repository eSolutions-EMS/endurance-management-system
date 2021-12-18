using EnduranceJudge.Core.Mappings;
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

        public Athlete Save(IAthleteState state, int countryId)
        {
            this.Validate<AthleteException>(() =>
            {
                state.FirstName.IsRequired(Words.FIRST_NAME);
                state.LastName.IsRequired(Words.LAST_NAME);
                state.Category.IsRequired(Words.CATEGORY);
                countryId.IsRequired(Entities.COUNTRY);
            });

            var athlete = this.state.Athletes.FindDomain(state.Id);
            if (athlete == null)
            {
                var country = this.state.Countries.FindDomain(countryId);
                athlete = new Athlete(state, country);
                this.state.Athletes.AddOrUpdate(athlete);
                this.UpdateParticipants(athlete);
            }
            else
            {
                athlete.Category = state.Category;
                athlete.Club = state.Club;
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
