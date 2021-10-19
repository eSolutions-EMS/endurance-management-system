using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Core.Exceptions;
using EnduranceJudge.Domain.Core.Extensions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Horses;
using static EnduranceJudge.Localization.DesktopStrings;

namespace EnduranceJudge.Domain.Aggregates.Configuration
{
    public class HorsesManager : ManagerObjectBase
    {
        private readonly IState state;
        internal HorsesManager(IState state)
        {
            this.state = state;
        }

        public Horse Save(IHorseState state)
        {
            this.Validate<HorseException>(() =>
            {
                state.Name.IsRequired(NAME);
            });

            var horse = this.state.Horses.FindDomain(state.Id);
            if (horse == null)
            {
                horse = new Horse(state);
                this.state.Horses.AddOrUpdate(horse);
                this.UpdateParticipants(horse);
            }
            else
            {
                horse.FeiId = state.FeiId;
                horse.Club = state.Club;
                horse.IsStallion = state.IsStallion;
                horse.Breed = state.Breed;
                horse.TrainerFeiId = state.TrainerFeiId;
                horse.TrainerFirstName = state.TrainerFirstName;
                horse.TrainerLastName = state.TrainerLastName;
                horse.Name = state.Name;
            }

            return horse;
        }

        public void Remove(int id)
        {
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
