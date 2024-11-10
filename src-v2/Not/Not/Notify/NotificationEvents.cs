using Not.Events;
using Not.Localization;

namespace Not.Notifier;

public abstract class NotifyEvent
{
    protected NotifyEvent(string message)
    {
        Message = message;
    }

    public string Message { get; }
}

public class Informed : NotifyEvent
{
    public Informed(string message) : base(message.Localize()) { }
}

public class Warned : NotifyEvent
{
    public Warned(string message) : base(message.Localize()) { }
}

public class Succeeded : NotifyEvent
{
    public Succeeded(string message) : base(message.Localize()) { }
}

public class Failed : NotifyEvent
{
    public Failed(string message) : base(message.Localize()) { }
}
