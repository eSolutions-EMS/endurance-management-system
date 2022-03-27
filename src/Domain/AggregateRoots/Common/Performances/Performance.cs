using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Domain.State.Laps;
using EnduranceJudge.Domain.State.LapRecords;
using EnduranceJudge.Domain.State.Participations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.AggregateRoots.Common.Performances;

public class Performance : IAggregate, IPerformance
{
    public const int COMPULSORY_INSPECTION_TIME_OFFSET = -15;

    private readonly List<Lap> laps;
    private readonly List<LapRecord> timeRecords;

    public Performance(Participation participation, int index)
    {
        this.Participant = participation.Participant;
        this.timeRecords = this.Participant.LapRecords.ToList();
        this.Index = index;
        this.laps = participation.CompetitionConstraint.Laps.ToList();
    }

    public Participant Participant { get; }

    public int Index { get; }
    public DateTime? RequiredInspectionTime
    {
        get
        {
            if (!this.CurrentRecord.IsRequiredInspectionRequired
                && !this.CurrentRecord.Lap.IsCompulsoryInspectionRequired)
            {
                return null;
            }
            var inspection = this.CurrentRecord.VetGateTime
                ?.AddMinutes(this.Lap.RestTimeInMins)
                .AddMinutes(COMPULSORY_INSPECTION_TIME_OFFSET);
            return inspection;
        }
    }

    public TimeSpan? RecoverySpan
        => this.CurrentRecord.VetGateTime - this.CurrentRecord.ArrivalTime;

    public TimeSpan? Time
        => this.CalculateLapTime(this.CurrentRecord, this.Lap);

    public double? AverageSpeed
    {
        get
        {
            var lapLengthInKm = this.Lap.LengthInKm;
            var totalHours = this.Time?.TotalHours;
            return  lapLengthInKm / totalHours;
        }
    }

    public double? AverageSpeedTotal
    {
        get
        {
            if (!this.Time.HasValue)
            {
                return null;
            }
            var totalTime = this.CalculateTotalTime();
            var totalAverageSpeed = this.TotalLength / totalTime.TotalHours;
            return totalAverageSpeed;
        }
    }

    public double TotalLength
        => this.TotalLaps
            .Select(x => x.LengthInKm)
            .Sum();

    public DateTime? NextStartTime
        => this.CurrentRecord.VetGateTime?.AddMinutes(this.CurrentRecord.Lap.RestTimeInMins);

    private TimeSpan? CalculateLapTime(LapRecord record, Lap lap)
        => lap.IsFinal
            ? record.VetGateTime - record.StartTime
            : record.ArrivalTime - record.StartTime;

    private TimeSpan CalculateTotalTime()
    {
        var totalHours = TimeSpan.Zero;
        for (var i = 0; i <= this.Index; i++)
        {
            var record = this.timeRecords[i];
            var lap = this.laps[i];
            var time = this.CalculateLapTime(record, lap);
            totalHours += time!.Value;
        }
        return totalHours;
    }

    public Lap Lap => this.laps[this.Index];
    private LapRecord CurrentRecord => this.timeRecords[this.Index];
    private IEnumerable<Lap> TotalLaps => this.laps.Take(this.Index + 1);
    private IEnumerable<LapRecord> TotalRecords => this.timeRecords.Take(this.Index + 1);

    public static DateTime CalculateStartTime(LapRecord record, Lap lap)
    {
        if (!record.VetGateTime.HasValue)
        {
            throw new Exception("Cannot calculate Start Time for next Lap, because the current is not complete.");
        }
        return record.VetGateTime.Value.AddMinutes(lap.RestTimeInMins);
    }

    public int Id => this.CurrentRecord.Id;
    public DateTime StartTime => this.CurrentRecord.StartTime;
    public DateTime? ArrivalTime => this.CurrentRecord.ArrivalTime;
    public DateTime? InspectionTime => this.CurrentRecord.InspectionTime;
    public DateTime? ReInspectionTime => this.CurrentRecord.ReInspectionTime;
    public bool IsReInspectionRequired => this.CurrentRecord.IsReInspectionRequired;
    public bool IsRequiredInspectionRequired => this.CurrentRecord.IsRequiredInspectionRequired;

    public static IEnumerable<Performance> GetAll(Participation participation)
    {
        var index = 0;
        foreach (var _ in participation.Participant.LapRecords)
        {
            yield return new Performance(participation, index);
            index++;
        }
    }
    public static Performance GetCurrent(Participation participation)
    {
        var index = participation.Participant.LapRecords.Count - 1;
        return new Performance(participation, index);
    }
}
