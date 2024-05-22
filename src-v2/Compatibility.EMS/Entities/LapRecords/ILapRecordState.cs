using NTS.Compabitility.EMS.Abstractions;
using System;

namespace Core.Domain.State.LapRecords;

public interface ILapRecordState : IIdentifiable
{
    DateTime StartTime { get; }
    DateTime? ArrivalTime { get; }
    DateTime? InspectionTime { get; }
    DateTime? ReInspectionTime { get; }
    bool IsReinspectionRequired { get; }
    bool IsRequiredInspectionRequired { get; }
}
