using Not.Localization;

namespace Not.Notify;

public abstract class NotifyEvent
{
    protected NotifyEvent(string message)
    {
        Message = message;
    }

    public string Message { get; }
}

public class Information : NotifyEvent
{
    public Information(string message)
        : base(message.Localize()) { }
}

public class Warning : NotifyEvent
{
    public Warning(string message)
        : base(message.Localize()) { }
}

public class Success : NotifyEvent
{
    public Success(string message)
        : base(message.Localize()) { }
}

public class Failure : NotifyEvent
{
    public Failure(string message)
        : base(message.Localize()) { }
}
