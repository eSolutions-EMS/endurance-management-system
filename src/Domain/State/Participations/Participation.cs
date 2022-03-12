using EnduranceJudge.Domain.Core.Exceptions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State.Competitions;
using EnduranceJudge.Domain.State.Participants;
using System.Collections.Generic;
using System.Linq;
using static EnduranceJudge.Localization.Strings;

namespace EnduranceJudge.Domain.State.Participations
{
    public class Participation : DomainBase<ParticipationException>
    {
        internal Participation() {}
        internal Participation(Participant participant, Competition competition) : base(GENERATE_ID)
        {
            this.Participant = participant;
            this.CompetitionConstraint = competition;
        }

        private List<int> competitionsIds = new();
        public Participant Participant { get; private set; }
        public Competition CompetitionConstraint { get; private set; }

        public double? Distance
            => this.CompetitionConstraint
                ?.Phases
                .Select(x => x.LengthInKm)
                .Sum();

        // TODO: In aggregate.
        internal void Add(Competition competition)
        {
            if (this.CompetitionsIds.Any())
            {
                if (this.CompetitionConstraint.Phases.Count != competition.Phases.Count)
                {
                    throw Helper.Create<ParticipantException>(
                        CANNOT_ADD_PARTICIPATION_DIFFERENT_PHASE_COUNT_MESSAGE,
                        competition.Name);
                }
                for (var phaseIndex = 0; phaseIndex < this.CompetitionConstraint.Phases.Count; phaseIndex++)
                {
                    var phaseConstraint = this.CompetitionConstraint.Phases[phaseIndex];
                    var phase = competition.Phases[phaseIndex];
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (phaseConstraint.LengthInKm != phase.LengthInKm)
                    {
                        throw Helper.Create<ParticipantException>(
                            CANNOT_ADD_PARTICIPATION_DIFFERENT_PHASE_LENGTHS_MESSAGE,
                            competition.Name,
                            this.CompetitionConstraint.Name,
                            phaseIndex + 1,
                            phase.LengthInKm,
                            phaseConstraint.LengthInKm);
                    }
                }
            }
            this.competitionsIds.Add(competition.Id);
        }
        internal void Remove(int competitionId)
        {
            this.competitionsIds.Remove(competitionId);
        }

        public IReadOnlyList<int> CompetitionsIds
        {
            get => this.competitionsIds.AsReadOnly();
            private set => this.competitionsIds = value.ToList();
        }

        public void __REMOVE_PERFORMANCES__()
        {
            this.Participant.__REMOVE_RECORDS__();
        }
    }
}
