using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State.Competitions;
using EnduranceJudge.Domain.State.Participations;
using EnduranceJudge.Domain.AggregateRoots.Common.Performances;
using EnduranceJudge.Domain.Core.Exceptions;
using EnduranceJudge.Domain.State.Laps;
using EnduranceJudge.Domain.State.LapRecords;
using EnduranceJudge.Domain.State.Participants;
using System;
using System.Linq;
using static EnduranceJudge.Localization.Strings;

namespace EnduranceJudge.Domain.AggregateRoots.Manager.Aggregates;

public class ParticipationsAggregate : IAggregate
{
    private readonly Competition competitionConstraint;
    private readonly Participation participation;

    internal ParticipationsAggregate(Participation participation)
    {
        this.Number = participation.Participant.Number;
        this.participation = participation;
        this.competitionConstraint = participation.CompetitionConstraint;
    }

    public int Number { get; }

    internal void Start()
    {
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
    internal void Add(Competition competition)
    {
        if (this.participation.CompetitionsIds.Any())
        {
            if (this.participation.CompetitionConstraint.Laps.Count != competition.Laps.Count)
            {
                throw Helper.Create<ParticipantException>(
                    CANNOT_ADD_PARTICIPATION_DIFFERENT_PHASE_COUNT_MESSAGE,
                    competition.Name);
            }
            for (var lapIndex = 0; lapIndex < this.participation.CompetitionConstraint.Laps.Count; lapIndex++)
            {
                var lapConstraint = this.participation.CompetitionConstraint.Laps[lapIndex];
                var lap = competition.Laps[lapIndex];
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (lapConstraint.LengthInKm != lap.LengthInKm)
                {
                    throw Helper.Create<ParticipantException>(
                        CANNOT_ADD_PARTICIPATION_DIFFERENT_PHASE_LENGTHS_MESSAGE,
                        competition.Name,
                        this.participation.CompetitionConstraint.Name,
                        lapIndex + 1,
                        lap.LengthInKm,
                        lapConstraint.LengthInKm);
                }
            }
        }
        this.participation.Add(competition.Id);
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

public static partial class AggregateExtensions
{
    public static ParticipationsAggregate Aggregate(this Participation participation)
    {
        return new ParticipationsAggregate(participation);
    }
}
