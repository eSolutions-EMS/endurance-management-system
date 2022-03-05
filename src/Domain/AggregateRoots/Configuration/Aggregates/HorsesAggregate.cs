using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.AggregateRoots.Configuration.Extensions;
using EnduranceJudge.Domain.Core.Exceptions;
using EnduranceJudge.Domain.Core.Extensions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Validation;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Horses;
using static EnduranceJudge.Localization.Translations.Messages.DomainValidation;
using static EnduranceJudge.Localization.Translations.Words;

namespace EnduranceJudge.Domain.AggregateRoots.Configuration.Aggregates
{
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
                this.UpdateParticipants(horse);
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
                    throw Helper.Create<HorseException>(CANNOT_REMOVE_USED_IN_PARTICIPANT);
                }
            }

            this.state.Horses.Remove(horse);
        }

        private void UpdateParticipants(Horse horse)
        {
            foreach (var participant in this.state.Participants)
            {
                if (participant.Horse.Equals(horse))
                {
                    participant.Horse.MapFrom(horse);
                }
            }
        }
    }
}
