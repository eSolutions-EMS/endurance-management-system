﻿using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State.Competitions;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Domain.State.Participations;
using EnduranceJudge.Domain.AggregateRoots.Common.Performances;
using EnduranceJudge.Domain.State.Laps;
using EnduranceJudge.Domain.State.TimeRecords;
using System;
using System.Linq;
using static EnduranceJudge.Domain.DomainConstants.ErrorMessages;

namespace EnduranceJudge.Domain.AggregateRoots.Manager.Aggregates
{
    public class ParticipantsAggregate : IAggregate
    {
        private readonly Competition competitionConstraint;
        private readonly Participant participant;

        // TODO: Rename to ParticipationsAggregate
        internal ParticipantsAggregate(Participation participation)
        {
            this.Number = participation.Participant.Number;
            this.participant = participation.Participant;
            this.competitionConstraint = participation.CompetitionConstraint;
        }

        public int Number { get; }

        // TODO: Move in StartNext?
        internal void Start()
        {
            if (this.participant.TimeRecords.Any())
            {
                throw new Exception(PARTICIPANT_HAS_ALREADY_STARTED);
            }
            var firstLap = this.competitionConstraint.Laps.FirstOrDefault();
            if (firstLap == null)
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
            var record = this.participant.TimeRecords.SingleOrDefault(x => x.Result == null);
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
            var currentRecord = this.participant.TimeRecords.LastOrDefault();
            if (currentRecord == null)
            {
                throw new Exception(CANNOT_START_NEXT_PERFORMANCE_NO_LAST_PERFORMANCE);
            }
            var startTime = Performance.CalculateStartTime(currentRecord, this.CurrentLap);
            return this.AddRecord(startTime);
        }

        private bool IsComplete
            => this.participant.TimeRecords.Count == this.competitionConstraint.Laps.Count
                && this.participant.TimeRecords.All(x => x.Result != null);

        private PerformancesAggregate AddRecord(DateTime startTime)
        {
            var record = new TimeRecord(FixDateForToday(startTime), this.NextLap);
            this.participant.Add(record);
            var aggregate = new PerformancesAggregate(record);
            return aggregate;
        }

        private Lap CurrentLap => this.competitionConstraint.Laps[this.participant.TimeRecords.Count];
        private Lap NextLap => this.competitionConstraint.Laps[this.participant.TimeRecords.Count + 1];

        // TODO: Remove after testing lap
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
