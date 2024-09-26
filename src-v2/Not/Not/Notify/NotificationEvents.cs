using Not.Events;

namespace Not.Notifier;

public abstract class NotifyEvent(string message) : IEvent
{
    public string Message { get; } = message;
}

public class Informed(string message) : NotifyEvent(message)
{
}

public class Warned(string message) : NotifyEvent(message)
{
}

public class Succeeded(string message) : NotifyEvent(message)
{
}

public class Failed(string message) : NotifyEvent(message)
{
}
