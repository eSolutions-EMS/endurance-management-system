using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State.Competitions;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Domain.State.Participations;
using EnduranceJudge.Domain.State.Performances;
using EnduranceJudge.Domain.State.Phases;
using EnduranceJudge.Domain.State.TimeRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using static EnduranceJudge.Domain.DomainConstants.ErrorMessages;

namespace EnduranceJudge.Domain.AggregateRoots.Manager.Aggregates
{
    public class ParticipantsAggregate : IAggregate
    {
        private readonly Competition competitionConstraint;
        private readonly List<TimeRecord> timeRecords;

        // TODO: Rename to ParticipationsAggregate
        internal ParticipantsAggregate(Participation participation)
        {
            this.Number = participation.Participant.Number;
            this.timeRecords = participation.Participant.TimeRecords.ToList();
            this.competitionConstraint = participation.CompetitionConstraint;
        }

        public int Number { get; }

        internal void Start()
        {
            if (this.timeRecords.Any())
            {
                throw new Exception(PARTICIPANT_HAS_ALREADY_STARTED);
            }
            var firstPhase = this.competitionConstraint.Phases.FirstOrDefault();
            if (firstPhase == null)
            {
                throw new Exception(CANNOT_START_COMPETITION_WITHOUT_PHASES);
            }
            this.AddRecord(this.competitionConstraint.StartTime);
        }
        internal void UpdatePerformance(DateTime time)
        {
            var record = this.GetCurrent() ?? this.CreateNext();
            record.Update(time);
        }
        internal PerformancesAggregate GetCurrent()
        {
            var record = this.timeRecords.SingleOrDefault(x => x.Result == null);
            if (record == null)
            {
                return null;
            }
            var recordsAggregate = new PerformancesAggregate(record);
            return recordsAggregate;
        }

        private PerformancesAggregate CreateNext()
        {
            if (this.IsComplete)
            {
                throw new Exception(CANNOT_START_NEXT_PERFORMANCE_PARTICIPATION_IS_COMPLETE);
            }
            var currentRecord = this.timeRecords.LastOrDefault();
            if (currentRecord == null)
            {
                throw new Exception(CANNOT_START_NEXT_PERFORMANCE_NO_LAST_PERFORMANCE);
            }
            var startTime = Performance.CalculateStartTime(currentRecord, this.CurrentPhase);
            return this.AddRecord(startTime);
        }

        private bool IsComplete
            => this.timeRecords.Count == this.competitionConstraint.Phases.Count
                && this.timeRecords.All(x => x.Result != null);

        private PerformancesAggregate AddRecord(DateTime startTime)
        {
            var record = new TimeRecord(FixDateForToday(startTime), this.NextPhase);
            this.timeRecords.Add(record);
            var aggregate = new PerformancesAggregate(record);
            return aggregate;
        }

        private Phase CurrentPhase => this.competitionConstraint.Phases[this.timeRecords.Count];
        private Phase NextPhase => this.competitionConstraint.Phases[this.timeRecords.Count + 1];

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
