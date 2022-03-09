using EnduranceJudge.Domain.AggregateRoots.Configuration.Extensions;
using EnduranceJudge.Domain.Core.Exceptions;
using EnduranceJudge.Domain.Core.Extensions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Athletes;
using EnduranceJudge.Domain.State.Horses;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Domain.State.Participations;
using System.Linq;
using static EnduranceJudge.Localization.Strings;

namespace EnduranceJudge.Domain.AggregateRoots.Configuration.Aggregates
{
    public class ParticipantsAggregate : IAggregate
    {
        private readonly IState state;

        internal ParticipantsAggregate(IState state)
        {
            this.state = state;
        }

        public Participant Save(IParticipantState participantState, int athleteId, int horseId)
        {
            this.state.ValidateThatEventHasNotStarted();

            var athlete = this.state.Athletes.FindDomain(athleteId);
            var horse = this.state.Horses.FindDomain(horseId);
            if (this.IsPartOfAnotherParticipant(athlete, participantState.Id))
            {
                throw Helper.Create<ParticipantException>(ALREADY_PARTICIPATING_MESSAGE, athlete.Name);
            }
            if (this.IsPartOfAnotherParticipant(horse, participantState.Id))
            {
                throw Helper.Create<ParticipantException>(ALREADY_PARTICIPATING_MESSAGE, horse.Name);
            }

            var participant = this.state.Participants.FindDomain(participantState.Id);
            if (participant == null)
            {
                participant = new Participant(athlete, horse, participantState);
                this.state.Participants.AddOrUpdate(participant);
            }
            else
            {
                participant.Athlete = athlete;
                participant.Horse = horse;
                participant.RfId = participantState.RfId;
                participant.MaxAverageSpeedInKmPh = participantState.MaxAverageSpeedInKmPh;
                participant.Number = participantState.Number;
            }

            return participant;
        }

        public void Remove(int id)
        {
            this.state.ValidateThatEventHasNotStarted();

            var participant = this.state.Participations.FindDomain(id);
            this.state.Participations.Remove(participant);
        }
        public void AddParticipation(int competitionId, int participantId)
        {
            this.state.ValidateThatEventHasNotStarted();

            var competition = this.state.Event.Competitions.FindDomain(competitionId);
            var participation = this.state.Participations.FirstOrDefault(x => x.Participant.Id == participantId);
            if (participation == null)
            {
                var participant = this.state.Participants.FindDomain(participantId);
                participation = new Participation(participant, competition);
            }
            participation.Add(competition);
            this.state.Participations.Add(participation);
        }

        private bool IsPartOfAnotherParticipant(Athlete athlete, int participantId)
            => this.state.Participants.Any(x => x.Athlete.Equals(athlete) && x.Id != participantId);

        private bool IsPartOfAnotherParticipant(Horse horse, int participantId)
            => this.state.Participants.Any(x => x.Horse.Equals(horse) && x.Id != participantId);

        public void __REVERT_START_PARTICIPATIONS__()
        {
            foreach (var participant in this.state.Participations)
            {
                participant.__REMOVE_PERFORMANCES__();
            }
        }
    }
}
