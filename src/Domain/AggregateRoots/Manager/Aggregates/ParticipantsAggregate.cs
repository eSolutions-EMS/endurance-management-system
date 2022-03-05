using EnduranceJudge.Domain.AggregateRoots.Manager.Performances;
using EnduranceJudge.Domain.Core.Exceptions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State.Competitions;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Domain.State.Participations;
using EnduranceJudge.Domain.State.Performances;
using EnduranceJudge.Domain.State.Phases;
using System;
using System.Linq;
using static EnduranceJudge.Localization.Translations.Messages;

namespace EnduranceJudge.Domain.AggregateRoots.Manager.Participants
{
    public class ParticipantsAggregate : IAggregate
    {
        private readonly Competition competition;
        private readonly Participation participation;

        internal ParticipantsAggregate(Participant participant)
        {
            this.Number = participant.Number;
            this.participation = participant.Participation;
            this.competition = this.participation.Competitions.FirstOrDefault();
            if (this.competition == null)
            {
                throw DomainExceptionBase.Create<ParticipantException>(
                    PARTICIPANT_CANNOT_START_NO_COMPETITION_TEMPLATE,
                    this.Number);
            }
        }

        public int Number { get; }

        internal void Start()
        {
            if (this.participation.Performances.Any())
            {
                throw DomainExceptionBase.Create<ParticipationException>(HAS_ALREADY_STARTED);
            }
            var firstPhase = this.competition.Phases.FirstOrDefault();
            if (firstPhase == null)
            {
                throw DomainExceptionBase.Create<ParticipantException>(CANNOT_START_COMPETITION_WITHOUT_PHASES);
            }
            this.AddPerformance(firstPhase, this.competition.StartTime);
        }
        internal void UpdatePerformance(DateTime time)
        {
            var performance = this.GetActivePerformance() ?? this.StartNext();
            performance.Update(time);
        }
        internal PerformancesAggregate GetActivePerformance()
        {
            var activePerformance = this.participation.Performances.SingleOrDefault(x => x.Result == null);
            if (activePerformance == null)
            {
                return null;
            }
            var currentManager = new PerformancesAggregate(activePerformance);
            return currentManager;
        }

        private PerformancesAggregate StartNext()
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

        private PerformancesAggregate AddPerformance(Phase phase, DateTime startTime)
        {
            var previousLengths = this.participation.Performances.Select(x => x.Phase.LengthInKm);
            var previousTimes = this.participation.Performances.Select(x => x.Time!.Value);
            var performance = new Performance(phase, FixDateForToday(startTime), previousLengths, previousTimes);
            this.participation.Add(performance);
            var manager = new PerformancesAggregate(performance);
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
