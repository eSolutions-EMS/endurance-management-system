namespace EMS.Domain;

public interface IPhaseEvent
{
    ICoreIdentifier CoreId { get; }
    PhaseEventType Type { get; }
    Timestamp Timestamp { get; }
}

public enum PhaseEventType
{
    Arrive = 1,
    In = 2,
}