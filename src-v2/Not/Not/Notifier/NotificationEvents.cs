using Not.Events;

namespace Not.Notifier;

public abstract class NotificationEvent(string message) : IEvent
{
    public string Message { get; } = message;
}

public class Informed(string message) : NotificationEvent(message)
{
}

public class Warned(string message) : NotificationEvent(message)
{
}

public class Succeeded(string message) : NotificationEvent(message)
{
}

public class Failed(string message) : NotificationEvent(message)
{
}
