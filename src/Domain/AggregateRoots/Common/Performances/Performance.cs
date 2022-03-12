using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Domain.State.Phases;
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
    private readonly List<Phase> phases;
    private readonly List<TimeRecord> timeRecords;

    public Performance(Participant participant, IEnumerable<Phase> phases, int index)
    {
        this.Participant = participant;
        this.index = index;
        this.phases = phases.ToList();
        this.timeRecords = participant.TimeRecords.ToList();
    }

    public Participant Participant { get; }

    public DateTime? RequiredInspectionTime
    {
        get
        {
            var inspection = this.CurrentRecord.VetGateTime
                ?.AddMinutes(this.CurrentPhase.RestTimeInMins)
                .AddMinutes(COMPULSORY_INSPECTION_TIME_OFFSET);
            return inspection;
        }
    }

    public TimeSpan? RecoverySpan
        => this.CurrentRecord.VetGateTime - this.CurrentRecord.ArrivalTime;

    public TimeSpan? Time
        => this.CalculateLapTime(this.CurrentRecord, this.CurrentPhase);

    public double? AverageSpeed
    {
        get
        {
            var phaseLengthInKm = this.CurrentPhase.LengthInKm;
            var totalHours = this.Time?.TotalHours;
            return  phaseLengthInKm / totalHours;
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
        => this.TotalPhases
            .Select(x => x.LengthInKm)
            .Sum();

    private TimeSpan? CalculateLapTime(TimeRecord record, Phase phase)
        => phase.IsFinal
            ? record.VetGateTime - record.StartTime
            : record.ArrivalTime - record.StartTime;

    private TimeSpan CalculateTotalTime()
    {
        var totalHours = TimeSpan.Zero;
        for (var i = 0; i <= this.index; i++)
        {
            var record = this.timeRecords[i];
            var phase = this.phases[i];
            var time = this.CalculateLapTime(record, phase);
            totalHours += time!.Value;
        }
        return totalHours;
    }

    private Phase CurrentPhase => this.phases[this.index];
    private TimeRecord CurrentRecord => this.timeRecords[this.index];
    private IEnumerable<Phase> TotalPhases => this.phases.Take(this.index + 1);
    private IEnumerable<TimeRecord> TotalRecords => this.timeRecords.Take(this.index + 1);

    public static DateTime CalculateStartTime(TimeRecord record, Phase phase)
    {
        if (!record.VetGateTime.HasValue)
        {
            throw new Exception("Cannot calculate Start Time for next Phase, because the current is not complete.");
        }
        return record.VetGateTime.Value.AddMinutes(phase.RestTimeInMins);
    }

    public int Id => this.CurrentRecord.Id;
    public DateTime StartTime => this.CurrentRecord.StartTime;
    public DateTime? ArrivalTime => this.CurrentRecord.ArrivalTime;
    public DateTime? InspectionTime => this.CurrentRecord.InspectionTime;
    public DateTime? ReInspectionTime => this.CurrentRecord.ReInspectionTime;
    public bool IsReInspectionRequired => this.CurrentRecord.IsReInspectionRequired;
    public bool IsRequiredInspectionRequired => this.CurrentRecord.IsReInspectionRequired;
}
