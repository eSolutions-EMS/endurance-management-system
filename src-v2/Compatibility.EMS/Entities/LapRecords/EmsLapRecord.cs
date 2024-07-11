using NTS.Compatibility.EMS.Abstractions;
using NTS.Compatibility.EMS.Entities.Laps;
using NTS.Compatibility.EMS.Entities.Participants;
using NTS.Compatibility.EMS.Entities.Results;
using System.ComponentModel;

namespace NTS.Compatibility.EMS.Entities.LapRecords;

public class EmsLapRecord : EmsDomainBase<EmsLapRecordException>
{
    public EmsLapRecord() {}
    public EmsLapRecord(DateTime startTime, EmsLap lap) : base(GENERATE_ID)
    {
        this.StartTime = startTime;
        this.Lap = lap;
    }

    private DateTime? arrivalTime;
    private DateTime? inspectionTime;
    private DateTime? reInspectionTime;

    public EmsLap Lap { get; private set; }
    public DateTime StartTime { get; set; } // TODO: set to private/internal after testing
    public DateTime? ArrivalTime
    {
        get => this.arrivalTime;
        set => this.SetProperty(ref this.arrivalTime, value, nameof(this.ArrivalTime)); // TODO: set to private/internal after testing
    }
    public DateTime? InspectionTime
    {
        get => this.inspectionTime;
        set => this.SetProperty(ref this.inspectionTime, value, nameof(this.InspectionTime)); // TODO: set to private/internal after testing
    }
    public DateTime? ReInspectionTime
    {
        get => this.reInspectionTime;
        set => this.SetProperty(ref this.reInspectionTime, value, nameof(this.ReInspectionTime)); // TODO: set to private/internal after testing
    }
    public bool IsReinspectionRequired { get; internal set; }
    public bool IsRequiredInspectionRequired { get; internal set; }
    public EmsResult Result { get; internal set; }

    public DateTime? VetGateTime
        => this.ReInspectionTime ?? this.InspectionTime;
    public DateTime? NextStarTime
        => this.Lap.IsFinal
            ? null
            : this.VetGateTime?.AddMinutes(this.Lap.RestTimeInMins);
    public event PropertyChangedEventHandler PropertyChanged;

    public Dictionary<WitnessEventType, EmsRfidTag> Detected { get; private set; } = new();

    protected virtual void RaisePropertyChanged(string propertyName = null)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void SetProperty<T>(ref T property, T value, string name)
    {
        if (!value.Equals(property))
        {
            property = value;
            this.RaisePropertyChanged(name);
        }
    }
}

public enum WitnessEventType
{
    Invalid = 0,
    VetIn = 1,
    Arrival = 2,
}
