using Core.Domain.Common.Models;
using Core.Domain.Enums;
using Core.Domain.State.LapRecords;
using Core.Domain.State.Participations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Core.Domain.AggregateRoots.Common.Performances;

public class Performance : IAggregate, IPerformance, INotifyPropertyChanged
{
    public LapRecord Record { get; private set; }
    private readonly CompetitionType type;
    public const int COMPULSORY_INSPECTION_TIME_OFFSET = -15;

    public Performance(LapRecord record, CompetitionType type, double previousLength)
    {
        this.Record = record;
        this.type = type;
        this.TotalLength = this.Record.Lap.LengthInKm + previousLength;
        record.PropertyChanged += (_, _) => this.UpdateValues();
        this.UpdateValues();
    }

    private void UpdateValues()
    {
        this.NextStartTime = this.UpdateNextStartTime();
        this.RequiredInspectionTime = this.UpdateRequiredInspectionTime();
        this.RecoverySpan = this.UpdateRecoverySpan();
        this.Time = this.UpdateTime();
        this.AverageSpeed = this.UpdateAverageSpeed();
        this.AverageSpeedPhase = this.UpdateAverageSpeedPhase();
        this.RaisePropertyChanged(nameof(this.StartTime));
        this.RaisePropertyChanged(nameof(this.ArrivalTime));
        this.RaisePropertyChanged(nameof(this.InspectionTime));
        this.RaisePropertyChanged(nameof(this.ReInspectionTime));
        this.RaisePropertyChanged(nameof(this.RequiredInspectionTime));
        this.RaisePropertyChanged(nameof(this.RecoverySpan));
        this.RaisePropertyChanged(nameof(this.Time));
        this.RaisePropertyChanged(nameof(this.AverageSpeed));
        this.RaisePropertyChanged(nameof(this.AverageSpeedPhase));
        this.RaisePropertyChanged(nameof(this.NextStartTime));
    }
    public DateTime? RequiredInspectionTime { get; private set; }

    private DateTime? UpdateRequiredInspectionTime()
    {
        if (!this.Record.IsRequiredInspectionRequired
            && !this.Record.Lap.IsCompulsoryInspectionRequired)
        {
            return null;
        }
        var inspection = this.NextStartTime?.AddMinutes(COMPULSORY_INSPECTION_TIME_OFFSET);
        return inspection;
    }

    public double TotalLength { get; private set; }

    public TimeSpan? RecoverySpan { get; private set; }
    private TimeSpan? UpdateRecoverySpan()
        => this.Record.VetGateTime - this.Record.ArrivalTime;

    public TimeSpan? Time { get; private set; }
    private TimeSpan? UpdateTime()
        => Performance.GetCorrectTime(this.Record, this.type);

    public double? AverageSpeed { get; private set; }
    private double? UpdateAverageSpeed()
    {
        var lapLengthInKm = this.Record.Lap.LengthInKm;
        var totalHours = this.CalculateLoopTime()?.TotalHours;
        return  lapLengthInKm / totalHours;
    }

    public double? AverageSpeedPhase { get; private set; }
    public double? UpdateAverageSpeedPhase()
    {
        var phaseTime = this.CalculatePhaseTime();
        if (phaseTime == null)
        {
            return null;
        }
        var averageSpeedPhase = this.Record.Lap.LengthInKm / phaseTime!.Value.TotalHours;
        return averageSpeedPhase;
    }

    public DateTime? NextStartTime { get; private set; }
    public DateTime? UpdateNextStartTime()
        => this.Record.NextStarTime;

    private TimeSpan? CalculateLoopTime()
        => this.Record.ArrivalTime - this.Record.StartTime;

    private TimeSpan? CalculatePhaseTime()
        => this.Record.VetGateTime - this.Record.StartTime;

    public int Id => this.Record.Id;
    public DateTime StartTime => this.Record.StartTime;
    public DateTime? ArrivalTime => this.Record.ArrivalTime;
    public DateTime? InspectionTime => this.Record.InspectionTime;
    public DateTime? ReInspectionTime => this.Record.ReInspectionTime;
    public bool IsReinspectionRequired => this.Record.IsReinspectionRequired; //TODO: fix name in v4
    public bool IsRequiredInspectionRequired => this.Record.IsRequiredInspectionRequired;

    public static IEnumerable<Performance> GetAll(Participation participation)
    {
        var previousLength = 0d;
        foreach (var lapRecord in participation.Participant.LapRecords)
        {
            var performance = new Performance(lapRecord, participation.CompetitionConstraint.Type, previousLength);
            previousLength += lapRecord.Lap.LengthInKm;
            yield return performance;
        }
    }

    public static DateTime GetStartTime(Participation participation)
    {
        var lastRecord = participation.Participant.LapRecords.Last();
        var performance = new Performance(lastRecord, participation.CompetitionConstraint.Type, 0);
        return performance.NextStartTime ?? default;
    }

    internal static double? GetSpeed(LapRecord record, CompetitionType type)
    {
        var lapLengthInKm = record.Lap.LengthInKm;
        var totalHours = GetCorrectTime(record, type)?.TotalHours;
        return  lapLengthInKm / totalHours;
    }

    private static TimeSpan? GetCorrectTime(LapRecord record, CompetitionType type)
        => type == CompetitionType.National || record.Lap.IsFinal
            ? record.ArrivalTime - record.StartTime
            : record.VetGateTime - record.StartTime;

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void RaisePropertyChanged(string propertyName = null)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
