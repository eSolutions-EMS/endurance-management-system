using Not.Events;

namespace Not.Notify;

public static class NotificationEvents
{
    public static readonly Event<Information> Informed = new();
    public static readonly Event<Success> Succeded = new();
    public static readonly Event<Warning> Warned = new();
    public static readonly Event<Failure> Failed = new();
}
