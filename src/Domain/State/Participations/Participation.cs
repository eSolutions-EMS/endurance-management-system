using EnduranceJudge.Core.Extensions;
using EnduranceJudge.Domain.Core.Extensions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State.Competitions;
using EnduranceJudge.Domain.State.Performances;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using static EnduranceJudge.Localization.Translations.Messages;

namespace EnduranceJudge.Domain.State.Participations
{
    public class Participation : DomainObjectBase<ParticipationException>
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

        internal void Add(Competition competition) => this.Validate(() =>
        {
            if (this.Competitions.Any())
            {
                var firstCompetition = this.competitions.First();
                var newCompetitionName = competition.Name;
                var existingCompetitionName = firstCompetition.Name;
                if (firstCompetition.Phases.Count != competition.Phases.Count)
                {
                    var message = string.Format(CANNOT_ADD_PARTICIPATION_DIFFERENT_PHASE_COUNT, newCompetitionName);
                    this.Throw(message);
                }
                for (var phaseIndex = 0; phaseIndex < firstCompetition.Phases.Count; phaseIndex++)
                {
                    var existingPhase = firstCompetition.Phases[phaseIndex];
                    var newPhase = competition.Phases[phaseIndex];
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (existingPhase.LengthInKm != newPhase.LengthInKm)
                    {
                        var message = string.Format(
                            CANNOT_ADD_PARTICIPATION_DIFFERENT_PHASE_LENGTHS,
                            newCompetitionName,
                            existingCompetitionName,
                            phaseIndex + 1,
                            newPhase.LengthInKm,
                            existingPhase.LengthInKm);
                        this.Throw(message);
                    }
                }
            }
            this.competitions.AddUnique(competition);
        });
        internal void Remove(Competition competition)
        {
            this.competitions.Remove(competition);
        }
        internal void Add(Performance performance)
        {
            this.performances.AddUnique(performance);
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
