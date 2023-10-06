namespace EMS.Domain;

public interface ICoreEvent
{
    CoreIdentifier CoreId { get; }
    CoreEventType Type { get; }
    Timestamp Timestamp { get; }
}

public enum CoreEventType
{
    Arrive = 1,
    In = 2,
}