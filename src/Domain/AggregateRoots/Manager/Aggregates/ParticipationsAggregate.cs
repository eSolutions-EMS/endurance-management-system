using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State.Competitions;
using EnduranceJudge.Domain.State.Participations;
using EnduranceJudge.Domain.AggregateRoots.Common.Performances;
using EnduranceJudge.Domain.Core.Exceptions;
using EnduranceJudge.Domain.State.Laps;
using EnduranceJudge.Domain.State.LapRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using static EnduranceJudge.Domain.DomainConstants.ErrorMessages;
using static EnduranceJudge.Localization.Strings;

namespace EnduranceJudge.Domain.AggregateRoots.Manager.Aggregates;

public class ParticipationsAggregate : IAggregate
{
    private readonly Competition competitionConstraint;
    private readonly Participation participation;

    // TODO: Rename to ParticipationsAggregate
    internal ParticipationsAggregate(Participation participation)
    {
        this.Number = participation.Participant.Number;
        this.participation = participation;
        this.competitionConstraint = participation.CompetitionConstraint;
    }

    public int Number { get; }

    internal IEnumerable<Performance> GetAllPerformances()
    {
        for (var i = 0; i < this.participation.Participant.LapRecords.Count; i++)
        {
            var performance = new Performance(this.participation, i);
            yield return performance;
        }
    }

    // TODO: Move in StartNext?
    internal void Start()
    {
        // TODO: remove
        if (this.participation.Participant.LapRecords.Any())
        {
            throw new Exception(PARTICIPANT_HAS_ALREADY_STARTED);
        }
        // TODO: Move check in validation
        var firstLap = this.competitionConstraint.Laps.FirstOrDefault();
        if (firstLap == null)
        {
            throw new Exception(CANNOT_START_COMPETITION_WITHOUT_PHASES);
        }
        this.AddRecord(this.competitionConstraint.StartTime);
    }
    internal void Update(DateTime time)
    {
        var record = this.GetCurrent() ?? this.CreateNext();
        record.Update(time);
    }
    internal LapRecordsAggregate GetCurrent()
    {
        var record = this.participation.Participant.LapRecords.SingleOrDefault(x => x.Result == null);
        if (record == null)
        {
            return null;
        }
        var recordsAggregate = new LapRecordsAggregate(record);
        return recordsAggregate;
    }

    private LapRecordsAggregate CreateNext()
    {
        var currentRecord = this.participation.Participant.LapRecords.LastOrDefault();
        if (this.NextLap == null)
        {
            throw Helper.Create<ParticipationException>(PARTICIPATION_HAS_ENDED_MESSAGE);
        }
        var startTime = Performance.CalculateStartTime(currentRecord, this.CurrentLap);
        return this.AddRecord(startTime);
    }

    private LapRecordsAggregate AddRecord(DateTime startTime)
    {
        var record = new LapRecord(FixDateForToday(startTime), this.NextLap);
        this.participation.Participant.Add(record);
        var lapsAggregate = new LapRecordsAggregate(record);
        return lapsAggregate;
    }

    private Lap CurrentLap => this.competitionConstraint.Laps[this.participation.Participant.LapRecords.Count - 1];
    private Lap NextLap
        => this.competitionConstraint.Laps.Count > this.participation.Participant.LapRecords.Count
            ? this.competitionConstraint.Laps[this.participation.Participant.LapRecords.Count]
            : null;

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