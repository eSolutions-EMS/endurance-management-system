using Not.Localization;

namespace Not.Notify;

public abstract class NotifyPayloads
{
    protected NotifyPayloads(string message)
    {
        Message = message;
    }

    public string Message { get; }
}

public class Information : NotifyPayloads
{
    public Information(string message)
        : base(message.Localize()) { }
}

public class Warning : NotifyPayloads
{
    public Warning(string message)
        : base(message.Localize()) { }
}

public class Success : NotifyPayloads
{
    public Success(string message)
        : base(message.Localize()) { }
}

public class Failure : NotifyPayloads
{
    public Failure(string message)
        : base(message.Localize()) { }
}
