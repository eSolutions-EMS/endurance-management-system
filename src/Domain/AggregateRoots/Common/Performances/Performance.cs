using EnduranceJudge.Domain.Annotations;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.State.Competitions;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Domain.State.Laps;
using EnduranceJudge.Domain.State.LapRecords;
using EnduranceJudge.Domain.State.Participations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace EnduranceJudge.Domain.AggregateRoots.Common.Performances;

public class Performance : IAggregate, IPerformance, INotifyPropertyChanged
{
    public const int COMPULSORY_INSPECTION_TIME_OFFSET = -15;

    private readonly List<Lap> laps;
    private readonly List<LapRecord> timeRecords;
    private readonly Competition competitionConstraint;

    public Performance(Participation participation, int index)
    {
        this.Participant = participation.Participant;
        this.competitionConstraint = participation.CompetitionConstraint;
        this.timeRecords = this.Participant.LapRecords.ToList();
        this.Index = index;
        this.laps = participation.CompetitionConstraint.Laps.ToList();

        this.SubscribeToLapRecordChanges(this.Participant.LapRecords);
        this.UpdateValues();
    }

    private void SubscribeToLapRecordChanges(IEnumerable<INotifyPropertyChanged> items)
    {
        foreach (var item in items)
        {
            item.PropertyChanged += (_, _) =>  this.UpdateValues();
        }
    }

    private void UpdateValues()
    {
        this.RequiredInspectionTime = this.UpdateRequiredInspectionTime();
        this.RecoverySpan = this.UpdateRecoverySpan();
        this.Time = this.UpdateTime();
        this.AverageSpeed = this.UpdateAverageSpeed();
        this.AverageSpeedTotal = this.UpdateAverageSpeedTotal();
        this.NextStartTime = this.UpdateNextStartTime();
        // this.ArrivalTime = this.LatestRecord.StartTime;
        this.RaisePropertyChanged(nameof(this.StartTime));
        this.RaisePropertyChanged(nameof(this.ArrivalTime));
        this.RaisePropertyChanged(nameof(this.InspectionTime));
        this.RaisePropertyChanged(nameof(this.ReInspectionTime));
        this.RaisePropertyChanged(nameof(this.RequiredInspectionTime));
        this.RaisePropertyChanged(nameof(this.RecoverySpan));
        this.RaisePropertyChanged(nameof(this.Time));
        this.RaisePropertyChanged(nameof(this.AverageSpeed));
        this.RaisePropertyChanged(nameof(this.AverageSpeedTotal));
        this.RaisePropertyChanged(nameof(this.NextStartTime));
    }

    public Participant Participant { get; }

    public int Index { get; }
    public DateTime? RequiredInspectionTime { get; private set; }

    private DateTime? UpdateRequiredInspectionTime()
    {
        if (!this.LatestRecord.IsRequiredInspectionRequired
            && !this.LatestRecord.Lap.IsCompulsoryInspectionRequired)
        {
            return null;
        }
        var inspection = this.NextStartTime?.AddMinutes(COMPULSORY_INSPECTION_TIME_OFFSET);
        return inspection;
    }

    public TimeSpan? RecoverySpan { get; private set; }
    private TimeSpan? UpdateRecoverySpan()
        => this.LatestRecord.VetGateTime - this.LatestRecord.ArrivalTime;

    public TimeSpan? Time { get; private set; }
    private TimeSpan? UpdateTime()
        => this.CalculateLapTime(this.LatestRecord, this.LatestLap);

    public double? AverageSpeed { get; private set; }
    private double? UpdateAverageSpeed()
    {
        var lapLengthInKm = this.LatestLap.LengthInKm;
        var totalHours = this.Time?.TotalHours;
        return  lapLengthInKm / totalHours;
    }

    public double? AverageSpeedTotal { get; private set; }
    public double? UpdateAverageSpeedTotal()
    {
        if (!this.Time.HasValue)
        {
            return null;
        }
        var totalTime = this.CalculateTotalTime();
        var totalAverageSpeed = this.TotalLength / totalTime.TotalHours;
        return totalAverageSpeed;
    }

    public double TotalLength
        => this.TotalLaps
            .Select(x => x.LengthInKm)
            .Sum();

    public DateTime? NextStartTime { get; private set; } 
    public DateTime? UpdateNextStartTime()
        => this.LatestRecord.NextStarTime;

    private TimeSpan? CalculateLapTime(LapRecord record, Lap lap)
        => this.competitionConstraint.Type == CompetitionType.National || lap.IsFinal
            ? record.ArrivalTime - record.StartTime
            : record.VetGateTime - record.StartTime;

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

    public Lap LatestLap => this.laps[this.Index];
    private LapRecord LatestRecord 
        => this.timeRecords[this.Index];
    
    private IEnumerable<Lap> TotalLaps => this.laps.Take(this.Index + 1);

    public int Id => this.LatestRecord.Id;
    public DateTime StartTime => this.LatestRecord.StartTime;
    public DateTime? ArrivalTime => this.LatestRecord.ArrivalTime;
    public DateTime? InspectionTime => this.LatestRecord.InspectionTime;
    public DateTime? ReInspectionTime => this.LatestRecord.ReInspectionTime;
    public bool IsReInspectionRequired => this.LatestRecord.IsReInspectionRequired;
    public bool IsRequiredInspectionRequired => this.LatestRecord.IsRequiredInspectionRequired;

    public static IEnumerable<Performance> GetAll(Participation participation)
    {
        var index = 0;
        foreach (var _ in participation.Participant.LapRecords)
        {
            yield return new Performance(participation, index);
            index++;
        }
    }
    public event PropertyChangedEventHandler PropertyChanged;
    [NotifyPropertyChangedInvocator]
    protected virtual void RaisePropertyChanged(string propertyName = null)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
