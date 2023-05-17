using Core.Domain.Core.Exceptions;
using Core.Domain.Core.Models;
using Core.Domain.State;
using Core.Domain.State.Horses;
using Core.Domain.Validation;
using Core.Domain.AggregateRoots.Configuration.Extensions;
using Core.Domain.Core.Extensions;
using static Core.Localization.Strings;

namespace Core.Domain.AggregateRoots.Configuration.Aggregates;

public class HorsesAggregate : IAggregate
{
    private readonly IState state;
    private readonly Validator<HorseException> validator;

    internal HorsesAggregate(IState state)
    {
        this.state = state;
        this.validator = new Validator<HorseException>();
    }

    public Horse Save(IHorseState horseState)
    {
        this.validator.IsRequired(horseState.Name, NAME);

        var horse = this.state.Horses.FindDomain(horseState.Id);
        if (horse == null)
        {
            horse = new Horse(horseState);
            this.state.Horses.AddOrUpdate(horse);
        }
        else
        {
            horse.FeiId = horseState.FeiId;
            horse.Club = horseState.Club;
            horse.IsStallion = horseState.IsStallion;
            horse.Breed = horseState.Breed;
            horse.TrainerFeiId = horseState.TrainerFeiId;
            horse.TrainerFirstName = horseState.TrainerFirstName;
            horse.TrainerLastName = horseState.TrainerLastName;
            horse.Name = horseState.Name;
        }

        return horse;
    }

    public void Remove(int id)
    {
        this.state.ValidateThatEventHasNotStarted();

        var horse = this.state.Horses.FindDomain(id);
        foreach (var participant in this.state.Participants)
        {
            if (participant.Athlete.Equals(horse))
            {
                throw Helper.Create<HorseException>(CANNOT_REMOVE_USED_IN_PARTICIPANT_MESSAGE);
            }
        }
        this.state.Horses.Remove(horse);
    }
}
