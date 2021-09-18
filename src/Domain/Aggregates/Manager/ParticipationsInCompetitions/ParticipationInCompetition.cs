using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.Aggregates.Manager.DTOs;
using EnduranceJudge.Domain.Aggregates.Manager.ParticipationsInPhases;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Enums;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.Aggregates.Manager.ParticipationsInCompetitions
{
    public class ParticipationInCompetition : DomainBase<ParticipationInCompetitionException>
    {
        private const string NEXT_PHASE_IS_NULL_MESSAGE = "cannot start - there is no Next Phase.";
        private const string CURRENT_PHASE_IS_NULL_MESSAGE = "cannot complete - no current phase.";

        private List<ParticipationInPhase> participationsInPhases = new();

        private ParticipationInCompetition()
        {
        }

        public CompetitionDto Competition { get; private set; }
        public Category Category { get; private set; }
        public int? MaxAverageSpeedInKpH { get; private set; }
        public IReadOnlyList<ParticipationInPhase> ParticipationsInPhases
        {
            get => this.participationsInPhases.AsReadOnly();
            private set => this.participationsInPhases = value.ToList();
        }

        public ParticipationInPhase CurrentPhase
            => this.participationsInPhases.SingleOrDefault(participation => !participation.IsComplete);
        public bool IsComplete => this.Competition.Phases.Count == this.participationsInPhases.Count;
        public CompetitionType CompetitionType => this.Competition.Type;
        public bool HasExceededSpeedRestriction => this.AverageSpeedInKpH > this.MaxAverageSpeedInKpH;
        public double? AverageSpeedInKpH
        {
            get
            {
                var completedPhases = this.participationsInPhases
                    .Where(x => x.IsComplete)
                    .ToList();

                if (!completedPhases.Any())
                {
                    return null;
                }

                var averageSpeedInPhases = this.CompetitionType == CompetitionType.International
                    ? completedPhases.Select(x => x.AverageSpeedForPhaseInKpH!.Value)
                    : completedPhases.Select(x => x.AverageSpeedForLoopInKpH!.Value);

                var averageSpeedSum = averageSpeedInPhases.Aggregate(0d, (sum, average) => sum + average);
                var phasesCount = this.participationsInPhases.Count;

                return averageSpeedSum / phasesCount;
            }
        }

        internal void StartPhase()
            => this.Validate(() =>
            {
                var nextPhase = this.Competition.Phases
                    .OrderBy(x => x.OrderBy)
                    .Skip(this.participationsInPhases.Count)
                    .FirstOrDefault()
                    .IsNotDefault(NEXT_PHASE_IS_NULL_MESSAGE);

                var restTime = this.CurrentPhase?.Phase[this.Category].RestTimeInMinutes;
                var nextStartTime = this.CurrentPhase?.ReInspectionTime?.AddMinutes(restTime!.Value)
                    ?? this.CurrentPhase?.InspectionTime?.AddMinutes(restTime!.Value)
                    ?? this.Competition.StartTime;

                var participation = new ParticipationInPhase(nextPhase, nextStartTime);
                this.participationsInPhases.Add(participation);
            });
        internal void CompleteSuccessful()
            => this.Validate(() =>
            {
                this.CurrentPhase
                    .IsNotDefault(CURRENT_PHASE_IS_NULL_MESSAGE)
                    .CompleteSuccessful();

                this.StartPhase();
            });
        internal void CompleteUnsuccessful(string code)
            => this.Validate(() =>
            {
                this.CurrentPhase
                    .IsNotDefault(CURRENT_PHASE_IS_NULL_MESSAGE)
                    .CompleteUnsuccessful(code);

                this.StartPhase();
            });

    }
}
