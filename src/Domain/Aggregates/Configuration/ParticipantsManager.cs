using EnduranceJudge.Domain.Core.Exceptions;
using EnduranceJudge.Domain.Core.Extensions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Athletes;
using EnduranceJudge.Domain.State.Horses;
using EnduranceJudge.Domain.State.Participants;
using System.Linq;
using static EnduranceJudge.Localization.DesktopStrings;

namespace EnduranceJudge.Domain.Aggregates.Configuration
{
    public class ParticipantsManager : ManagerObjectBase
    {
        private readonly IState state;

        internal ParticipantsManager(IState state)
        {
            this.state = state;
        }

        public Participant Save(IParticipantState state, int athleteId, int horseId)
        {
            var athlete = this.state.Athletes.FindDomain(athleteId);
            var horse = this.state.Horses.FindDomain(horseId);
            this.Validate<ParticipantException>(() =>
            {
                if (this.IsPartOfAnotherParticipant(athlete, state.Id))
                {
                    var message = string.Format(ALREADY_PARTICIPATING_TEMPLATE, athlete.Name);
                    throw new DomainException(message);
                }
                if (this.IsPartOfAnotherParticipant(horse, state.Id))
                {
                    var message = string.Format(ALREADY_PARTICIPATING_TEMPLATE, horse.Name);
                    throw new DomainException(message);
                }
            });

            var participant = this.state.Participants.FindDomain(state.Id);
            if (participant == null)
            {
                participant = new Participant(athlete, horse, state);
                this.state.Participants.AddOrUpdate(participant);
            }
            else
            {
                participant.Athlete = athlete;
                participant.Horse = horse;
                participant.RfId = state.RfId;
                participant.MaxAverageSpeedInKmPh = state.MaxAverageSpeedInKmPh;
                participant.Number = state.Number;
            }

            return participant;
        }

        public void Remove(int id)
        {
            var participant = this.state.Participants.FindDomain(id);
            this.state.Participants.Remove(participant);
        }

        private bool IsPartOfAnotherParticipant(Athlete athlete, int participantId)
            => this.state.Participants.Any(x => x.Athlete.Equals(athlete) && x.Id != participantId);

        private bool IsPartOfAnotherParticipant(Horse horse, int participantId)
            => this.state.Participants.Any(x => x.Horse.Equals(horse) && x.Id != participantId);
    }
}
