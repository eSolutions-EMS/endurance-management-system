using Core.Domain.Core.Exceptions;
using Core.Domain.Core.Models;
using Core.Domain.State;
using Core.Domain.State.Athletes;
using Core.Domain.Validation;
using Core.Domain.AggregateRoots.Configuration.Extensions;
using Core.Domain.Core.Extensions;
using static Core.Localization.Strings;

namespace Core.Domain.AggregateRoots.Configuration.Aggregates;

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
        this.validator.IsRequired(countryId, COUNTRY_ENTITY);

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
            athlete.FirstName = athleteState.FirstName;
            athlete.LastName = athleteState.LastName;
            athlete.FeiId = athleteState.FeiId;
            if (athlete.Country?.Id != countryId)
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
        foreach (var participant in this.state.Participants)
        {
            if (participant.Athlete.Equals(athlete))
            {
                throw Helper.Create<AthleteException>(CANNOT_REMOVE_USED_IN_PARTICIPANT_MESSAGE);
            }
        }
        this.state.Athletes.Remove(athlete);
    }
}
