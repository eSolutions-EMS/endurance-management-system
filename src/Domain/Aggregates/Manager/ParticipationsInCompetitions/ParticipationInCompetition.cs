using EnduranceJudge.Domain.Aggregates.Manager.DTOs;
using EnduranceJudge.Domain.Aggregates.Manager.PhasePerformances;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.Aggregates.Manager.ParticipationsInCompetitions
{
    public class ParticipationInCompetition : DomainObjectBase<ParticipationInCompetitionObjectException>
    {
        private readonly List<PhasePerformanceManagerOld> phasePerformances = new();
        private readonly List<PhaseDto> phases = new();

        private ParticipationInCompetition()
        {
        }

        public DateTime StartTime { get; private init; }
        public CompetitionType Type { get; private init; }
        public Category Category { get; private init; }
        public int? MaxAverageSpeedInKpH { get; private init; }
        public IReadOnlyList<PhaseDto> Phases
        {
            get => this.phases;
            private init => this.phases = value
                .OrderBy(x => x.OrderBy)
                .ToList();
        }
        public IReadOnlyList<PhasePerformanceManagerOld> PhasePerformances
        {
            get => this.phasePerformances.AsReadOnly();
            private init => this.PhasePerformances = value
                .OrderBy(x => x.PhaseOrderBy)
                .ToList();
        }

        public PhasePerformanceManagerOld CurrentPhasePerformance
            => this.PhasePerformances.SingleOrDefault(participation => !participation.IsComplete);
        public bool IsNotComplete
            => this.Phases?.Count != this.PhasePerformances?.Count
                || this.PhasePerformances.Any(x => !x.IsComplete);
        public bool HasExceededSpeedRestriction => this.AverageSpeedInKpH > this.MaxAverageSpeedInKpH;
        public double? AverageSpeedInKpH
        {
            get
            {
                var completedPhases = this.PhasePerformances
                    .Where(x => x.IsComplete)
                    .ToList();

                if (!completedPhases.Any())
                {
                    return null;
                }

                var averageSpeedInPhases = this.Type == CompetitionType.International
                    ? completedPhases.Select(x => x.AverageSpeedForPhaseInKpH!.Value)
                    : completedPhases.Select(x => x.AverageSpeedForLoopInKpH!.Value);

                var averageSpeedSum = averageSpeedInPhases.Aggregate(0d, (sum, average) => sum + average);
                var phasesCount = this.PhasePerformances.Count;

                return averageSpeedSum / phasesCount;
            }
        }

        internal void StartPhase()
            => this.Validate(() =>
            {
                var nextPhase = this.Phases
                    .Skip(this.PhasePerformances.Count)
                    .First();

                DateTime startTime;
                if (this.PhasePerformances.Any())
                {
                    var lastPhase = this.PhasePerformances.Last();
                    var restTime = lastPhase
                        .RestTimeInMinutes;
                    startTime = lastPhase.ReInspectionTime?.AddMinutes(restTime)
                        ?? lastPhase.InspectionTime!.Value.AddMinutes(restTime);
                }
                else
                {
                    startTime = this.StartTime;
                }

                var participation = new PhasePerformanceManagerOld(nextPhase, startTime);
                this.phasePerformances.Add(participation);
            });
        internal void CompleteSuccessful()
            => this.Validate(() =>
            {
                if (this.HasExceededSpeedRestriction)
                {
                    this.CompleteUnsuccessful("speedyGonzalez");
                }
                else
                {
                    this.CurrentPhasePerformance.CompleteSuccessful();
                    if (this.IsNotComplete)
                    {
                        this.StartPhase();
                    }
                }
            });
        internal void CompleteUnsuccessful(string code)
            => this.Validate(() =>
            {
                this.CurrentPhasePerformance.CompleteUnsuccessful(code);
            });

    }
}
