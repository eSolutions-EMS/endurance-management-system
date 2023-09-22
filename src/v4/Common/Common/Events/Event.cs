using Common.Exceptions;

namespace Common.Events;

public class Event
{
    private event EventHandler? GenericEvent;

    public void Invoke(object sender)
    {
        if (sender == null)
        {
            throw new CommonException("Invalid invocation attempt - sender is null");
        }
        GenericEvent?.Invoke(sender, EventArgs.Empty);
    }
    public void Subscrive(Action<object> action)
    {
        GenericEvent += (sender, _) => action(sender!);
    }
}

public class Event<T>
    where T : class
{
    private event EventHandler<T>? GenericEvent;

    public void Invoke(object sender, T data)
    {
        if (sender == null)
        {
            throw new CommonException("Invalid invocation attempt - sender is null");
        }
        GenericEvent?.Invoke(sender, data);
    }
    public void Subscrive(Action<object, T> action)
    {
        GenericEvent += (sender, data) => action(sender!, data);
    }
}

public class Event<T1, T2>
    where T1 : class
    where T2 : class
{
    private event EventHandler<(T1, T2)>? GenericEvent;

    public void Invoke(object sender, T1 data1, T2 data2)
    {
        if (sender == null)
        {
            throw new CommonException("Invalid invocation attempt - sender is null");
        }
        GenericEvent?.Invoke(sender, (data1, data2));
    }
    public void Subscrive(Action<object, T1, T2> action)
    {
        GenericEvent += (sender, data) => action(sender!, data.Item1, data.Item2);
    }
}