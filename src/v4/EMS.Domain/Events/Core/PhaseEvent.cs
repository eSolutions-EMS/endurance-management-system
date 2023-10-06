namespace EMS.Domain;

public interface IPhaseEvent
{
    CoreIdentifier CoreId { get; }
    PhaseEventType Type { get; }
    Timestamp Timestamp { get; }
}

public enum PhaseEventType
{
    Arrive = 1,
    In = 2,
}