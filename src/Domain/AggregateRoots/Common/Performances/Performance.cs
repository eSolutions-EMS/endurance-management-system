using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Domain.State.Laps;
using EnduranceJudge.Domain.State.TimeRecords;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.AggregateRoots.Common.Performances;

// TODO: rename to ???
public class Performance : IAggregate, IPerformance
{
    public const int COMPULSORY_INSPECTION_TIME_OFFSET = -15;

    private readonly int index;
    private readonly List<Lap> laps;
    private readonly List<TimeRecord> timeRecords;

    public Performance(Participant participant, IEnumerable<Lap> laps, int index)
    {
        this.Participant = participant;
        this.index = index;
        this.laps = laps.ToList();
        this.timeRecords = participant.TimeRecords.ToList();
    }

    public Participant Participant { get; }

    public DateTime? RequiredInspectionTime
    {
        get
        {
            var inspection = this.CurrentRecord.VetGateTime
                ?.AddMinutes(this.CurrentLap.RestTimeInMins)
                .AddMinutes(COMPULSORY_INSPECTION_TIME_OFFSET);
            return inspection;
        }
    }

    public TimeSpan? RecoverySpan
        => this.CurrentRecord.VetGateTime - this.CurrentRecord.ArrivalTime;

    public TimeSpan? Time
        => this.CalculateLapTime(this.CurrentRecord, this.CurrentLap);

    public double? AverageSpeed
    {
        get
        {
            var lapLengthInKm = this.CurrentLap.LengthInKm;
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

    private TimeSpan? CalculateLapTime(TimeRecord record, Lap lap)
        => lap.IsFinal
            ? record.VetGateTime - record.StartTime
            : record.ArrivalTime - record.StartTime;

    private TimeSpan CalculateTotalTime()
    {
        var totalHours = TimeSpan.Zero;
        for (var i = 0; i <= this.index; i++)
        {
            var record = this.timeRecords[i];
            var lap = this.laps[i];
            var time = this.CalculateLapTime(record, lap);
            totalHours += time!.Value;
        }
        return totalHours;
    }

    private Lap CurrentLap => this.laps[this.index];
    private TimeRecord CurrentRecord => this.timeRecords[this.index];
    private IEnumerable<Lap> TotalLaps => this.laps.Take(this.index + 1);
    private IEnumerable<TimeRecord> TotalRecords => this.timeRecords.Take(this.index + 1);

    public static DateTime CalculateStartTime(TimeRecord record, Lap lap)
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
    public bool IsRequiredInspectionRequired => this.CurrentRecord.IsReInspectionRequired;
}
