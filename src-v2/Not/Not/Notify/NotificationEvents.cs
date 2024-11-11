using Not.Events;

namespace Not.Notify;

public static class NotificationEvents
{
    public static readonly Event<Informed> Informed = new();
    public static readonly Event<Succeeded> Succeded = new();
    public static readonly Event<Warned> Warned = new();
    public static readonly Event<Failed> Failed = new();
}
