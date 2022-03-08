using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.AggregateRoots.Configuration.Extensions;
using EnduranceJudge.Domain.Core.Extensions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Validation;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Competitions;
using static EnduranceJudge.Localization.Strings.Words;

namespace EnduranceJudge.Domain.AggregateRoots.Configuration.Aggregates
{
    public class CompetitionsAggregate : IAggregate
    {
        private readonly IState state;
        private readonly Validator<CompetitionException> validator;

        internal CompetitionsAggregate(IState state)
        {
            this.state = state;
            this.validator = new Validator<CompetitionException>();
        }

        public Competition Save(ICompetitionState competitionState)
        {
            this.state.ValidateThatEventHasNotStarted();
            this.validator.IsRequired(competitionState.Type, TYPE);
            this.validator.IsRequired(competitionState.Name, NAME);
            this.validator.IsRequired(competitionState.StartTime, START_TIME);

            var competition = this.state.Event.Competitions.FindDomain(competitionState.Id);
            if (competition == null)
            {
                competition = new Competition(competitionState);
                this.state.Event.Save(competition);
            }
            else
            {
                competition.Name = competitionState.Name;
                competition.Type = competitionState.Type;
                competition.StartTime = competitionState.StartTime;
                this.UpdateParticipants(competition);
            }

            return competition;
        }

        public void RemoveParticipant(int competitionId, int participantId)
        {
            this.state.ValidateThatEventHasNotStarted();

            var competition = this.state.Event.Competitions.FindDomain(competitionId);
            var participant = this.state.Participants.FindDomain(participantId);
            participant.RemoveFrom(competition);
        }

        private void UpdateParticipants(Competition competition)
        {
            foreach (var participant in this.state.Participants)
            {
                foreach (var existingCompetition in participant.Participation.Competitions)
                {
                    if (existingCompetition.Equals(competition))
                    {
                        existingCompetition.MapFrom(competition);
                    }
                }
            }
        }
    }
}
