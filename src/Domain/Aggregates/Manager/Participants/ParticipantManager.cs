using EnduranceJudge.Domain.Aggregates.Manager.Performances;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State.Competitions;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Domain.State.Participations;
using EnduranceJudge.Domain.State.Performances;
using EnduranceJudge.Domain.State.Phases;
using System;
using System.Linq;
using static EnduranceJudge.Localization.Translations.Messages;

namespace EnduranceJudge.Domain.Aggregates.Manager.Participants
{
    public class ParticipantManager : ManagerObjectBase
    {
        private readonly Competition competition;
        private readonly Participation participation;

        internal ParticipantManager(Participant participant)
        {
            this.Number = participant.Number;
            this.participation = participant.Participation;
            this.competition = this.participation.Competitions.FirstOrDefault();
            if (this.competition == null)
            {
                var message = string.Format(PARTICIPANT_CANNOT_START_NO_COMPETITION_TEMPLATE, this.Number);
                this.Throw<ParticipantException>(message);
            }
        }

        public int Number { get; }

        internal void Start()
        {
            if (this.participation.Performances.Any())
            {
                this.Throw<ParticipationException>(HAS_ALREADY_STARTED);
            }
            var firstPhase = this.competition.Phases.FirstOrDefault();
            if (firstPhase == null)
            {
                this.Throw<ParticipantException>(CANNOT_START_COMPETITION_WITHOUT_PHASES);
            }
            this.AddPerformance(firstPhase, this.competition.StartTime);
        }
        internal void UpdatePerformance(DateTime time)
        {
            var performance = this.GetActivePerformance() ?? this.StartNext();
            performance.Update(time);
        }
        internal PerformanceManager GetActivePerformance()
        {
            var activePerformance = this.participation.Performances.SingleOrDefault(x => x.Result == null);
            if (activePerformance == null)
            {
                return null;
            }
            var currentManager = new PerformanceManager(activePerformance);
            return currentManager;
        }

        private PerformanceManager StartNext()
        {
            if (this.IsComplete)
            {
                throw new InvalidOperationException(CANNOT_START_NEXT_PERFORMANCE_PARTICIPATION_IS_COMPLETE);
            }
            var phase = this.competition
                .Phases
                .Skip(this.participation.Performances.Count)
                .FirstOrDefault();
            if (phase == null)
            {
                throw new InvalidOperationException(CANNOT_START_PERFORMANCE_NO_PHASE);
            }
            var previousPerformance = this.participation.Performances.LastOrDefault();
            if (previousPerformance == null)
            {
                throw new InvalidOperationException(CANNOT_START_NEXT_PERFORMANCE_NO_LAST_PERFORMANCE);
            }
            var startTime = previousPerformance.NextPerformanceStartTime;
            if (startTime == null)
            {
                throw new InvalidOperationException(CANNOT_START_PERFORMANCE_NO_START_TIME);
            }

            return this.AddPerformance(phase, startTime.Value);
        }

        private bool IsComplete
            => this.participation.Performances.Count == this.competition.Phases.Count
                && this.participation.Performances.All(x => x.Result != null);

        private PerformanceManager AddPerformance(Phase phase, DateTime startTime)
        {
            var performance = new Performance(phase, FixDateForToday(startTime));
            this.participation.Add(performance);
            var manager = new PerformanceManager(performance);
            return manager;
        }

        // TODO: Remove after testing phase
        private DateTime FixDateForToday(DateTime date)
        {
            var today = DateTime.Today;
            today = today.AddHours(date.Hour);
            today = today.AddMinutes(date.Minute);
            today = today.AddSeconds(date.Second);
            today = today.AddMilliseconds(date.Millisecond);
            return today;
        }
    }
}
