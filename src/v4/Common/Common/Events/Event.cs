using Common.Exceptions;

namespace Common.Events;

public class Event : IEventSubscriber, IEventEmitter
{
    private event EventHandler? GenericEvent;

    public void Emit(object sender)
    {
        SenderNullValidator.ThrowIfNull(sender);
        GenericEvent?.Invoke(sender, EventArgs.Empty);
    }
    public void Subscribe(Action<object> action)
    {
        GenericEvent += (sender, _) => action(sender!);
    }
}

public class Event<T> : IEventSubscriber<T>, IEventEmitter<T>
{
    private event EventHandler<T>? GenericEvent;

    public void Emit(object sender, T data)
    {
        SenderNullValidator.ThrowIfNull(sender);
        GenericEvent?.Invoke(sender, data);
    }
    public void Subscribe(Action<object, T> action)
    {
        GenericEvent += (sender, data) => action(sender!, data);
    }
}

public class Event<T1, T2> : IEventSubscriber<T1, T2>, IEventEmitter<T1, T2>
{
    private event EventHandler<(T1, T2)>? GenericEvent;

    public void Emit(object sender, T1 data1, T2 data2)
    {
        SenderNullValidator.ThrowIfNull(sender);
        GenericEvent?.Invoke(sender, (data1, data2));
    }
    public void Subscribe(Action<object, T1, T2> action)
    {
        GenericEvent += (sender, data) => action(sender!, data.Item1, data.Item2);
    }
}

internal class SenderNullValidator
{
    public static void ThrowIfNull(object? sender)
    {
        if (sender == null)
        {
            throw new CommonException("Invalid invocation attempt - sender is null");
        }
    }
}