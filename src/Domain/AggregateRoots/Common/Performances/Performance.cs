using EnduranceJudge.Domain.Annotations;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.State.LapRecords;
using EnduranceJudge.Domain.State.Participations;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace EnduranceJudge.Domain.AggregateRoots.Common.Performances;

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
        => this.type == CompetitionType.National || this.Record.Lap.IsFinal
            ? this.CalculateLoopTime()
            : this.CalculatePhaseTime();

    public double? AverageSpeed { get; private set; }
    private double? UpdateAverageSpeed()
    {
        var lapLengthInKm = this.Record.Lap.LengthInKm;
        var totalHours = this.Time?.TotalHours;
        return  lapLengthInKm / totalHours;
    }

    // TODO: Rename ot AverageSpeedPhase
    public double? AverageSpeedPhase { get; private set; }
    public double? UpdateAverageSpeedPhase()
    {
        if (!this.Time.HasValue)
        {
            return null;
        }
        var phaseTime = this.CalculatePhaseTime();
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
    public event PropertyChangedEventHandler PropertyChanged;
    [NotifyPropertyChangedInvocator]
    protected virtual void RaisePropertyChanged(string propertyName = null)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
