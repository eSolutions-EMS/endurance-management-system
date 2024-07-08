using System.ComponentModel;

namespace NTS.Judge.MAUI.Server.ACL.EMS;

public class EmsLapRecord : EmsDomainBase<EmsLapRecordException>, IEmsLapRecordState, INotifyPropertyChanged
{
    public EmsLapRecord() { }
    public EmsLapRecord(DateTime startTime, EmsLap lap) : base(default)
    {
        StartTime = startTime;
        Lap = lap;
    }

    private DateTime? arrivalTime;
    private DateTime? inspectionTime;
    private DateTime? reInspectionTime;

    public EmsLap Lap { get; private set; }
    public DateTime StartTime { get; set; } // TODO: set to private/internal after testing
    public DateTime? ArrivalTime
    {
        get => arrivalTime;
        set => SetProperty(ref arrivalTime, value, nameof(ArrivalTime)); // TODO: set to private/internal after testing
    }
    public DateTime? InspectionTime
    {
        get => inspectionTime;
        set => SetProperty(ref inspectionTime, value, nameof(InspectionTime)); // TODO: set to private/internal after testing
    }
    public DateTime? ReInspectionTime
    {
        get => reInspectionTime;
        set => SetProperty(ref reInspectionTime, value, nameof(ReInspectionTime)); // TODO: set to private/internal after testing
    }
    public bool IsReinspectionRequired { get; internal set; }
    public bool IsRequiredInspectionRequired { get; internal set; }
    public EmsResult Result { get; internal set; }

    public DateTime? VetGateTime
        => ReInspectionTime ?? InspectionTime;
    public DateTime? NextStarTime
        => Lap.IsFinal
            ? null
            : VetGateTime?.AddMinutes(Lap.RestTimeInMins);
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void RaisePropertyChanged(string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void SetProperty<T>(ref T property, T value, string name)
    {
        if (!value.Equals(property))
        {
            property = value;
            RaisePropertyChanged(name);
        }
    }
}
