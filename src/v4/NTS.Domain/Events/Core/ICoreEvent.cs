 namespace NTS.Domain;

public interface ICoreEvent
{
    CoreIdentifier CoreId { get; }
    CoreEventType Type { get; }
    Timestamp Timestamp { get; }
}