using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Aggregates.Configuration.Extensions;
using EnduranceJudge.Domain.Core.Exceptions;
using EnduranceJudge.Domain.Core.Extensions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Horses;
using static EnduranceJudge.Localization.Translations.Messages;
using static EnduranceJudge.Localization.Translations.Words;

namespace EnduranceJudge.Domain.Aggregates.Configuration
{
    public class HorsesManager : ManagerObjectBase
    {
        private readonly IState state;
        internal HorsesManager(IState state)
        {
            this.state = state;
        }

        public Horse Save(IHorseState horseState)
        {
            this.Validate<HorseException>(() =>
            {
                horseState.Name.IsRequired(NAME);
            });

            var horse = this.state.Horses.FindDomain(horseState.Id);
            if (horse == null)
            {
                horse = new Horse(horseState);
                this.state.Horses.AddOrUpdate(horse);
                this.UpdateParticipants(horse);
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
            this.Validate<HorseException>(() =>
            {
                foreach (var participant in this.state.Participants)
                {
                    if (participant.Athlete.Equals(horse))
                    {
                        throw new DomainException(CANNOT_REMOVE_USED_IN_PARTICIPANT);
                    }
                }
            });

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
