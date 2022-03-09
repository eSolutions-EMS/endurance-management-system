using EnduranceJudge.Domain.Core.Exceptions;
using EnduranceJudge.Domain.Core.Extensions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State.Competitions;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Domain.State.Performances;
using System.Collections.Generic;
using System.Linq;
using static EnduranceJudge.Localization.Strings;

namespace EnduranceJudge.Domain.State.Participations
{
    public class Participation : DomainBase<ParticipationException>
    {
        private List<Competition> competitions = new();
        private List<Performance> performances = new();

        public double AverageSpeedForLoopInKm
        {
            get
            {
                var finishedPerformances = this.performances
                    .Where(x => x.AverageSpeed.HasValue)
                    .ToList();
                var sum = finishedPerformances.Aggregate(
                    0d,
                    (sum, performance) => sum + performance.AverageSpeed!.Value);
                var average = sum / finishedPerformances.Count;
                return average;
            }
        }
        public double? Distance
            => this.Competitions.FirstOrDefault()
                ?.Phases
                .Select(x => x.LengthInKm)
                .Sum();

        internal void Add(Competition competition)
        {
            if (this.Competitions.Any())
            {
                var firstCompetition = this.competitions.First();
                var newCompetitionName = competition.Name;
                var existingCompetitionName = firstCompetition.Name;
                if (firstCompetition.Phases.Count != competition.Phases.Count)
                {
                    throw Helper.Create<ParticipantException>(
                        CANNOT_ADD_PARTICIPATION_DIFFERENT_PHASE_COUNT_MESSAGE,
                        newCompetitionName);
                }
                for (var phaseIndex = 0; phaseIndex < firstCompetition.Phases.Count; phaseIndex++)
                {
                    var existingPhase = firstCompetition.Phases[phaseIndex];
                    var newPhase = competition.Phases[phaseIndex];
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (existingPhase.LengthInKm != newPhase.LengthInKm)
                    {
                        throw Helper.Create<ParticipantException>(
                            CANNOT_ADD_PARTICIPATION_DIFFERENT_PHASE_LENGTHS_MESSAGE,
                            newCompetitionName,
                            existingCompetitionName,
                            phaseIndex + 1,
                            newPhase.LengthInKm,
                            existingPhase.LengthInKm);
                    }
                }
            }
            this.competitions.AddOrUpdate(competition);
        }
        internal void Remove(Competition competition)
        {
            this.competitions.Remove(competition);
        }
        internal void Add(Performance performance)
        {
            this.performances.AddOrUpdate(performance);
        }

        public IReadOnlyList<Competition> Competitions
        {
            get => this.competitions.AsReadOnly();
            private set => this.competitions = value.ToList();
        }
        public IReadOnlyList<Performance> Performances
        {
            get => this.performances.AsReadOnly();
            private set => this.performances = value.ToList();
        }

        public void __REMOVE_PERFORMANCES__()
        {
            this.performances.Clear();
        }
    }
}
