using Common.Conventions;
using Common.Exceptions;

namespace Common.Events;

public class Event : IEvent
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

public class Event<T> : IEvent<T>
    where T : class
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

public interface IEvent : ISingletonService
{
    void Emit(object sender);
    void Subscribe(Action<object> action);
}

public interface IEvent<T> : ISingletonService
    where T : class
{
    void Emit(object sender, T data);
    void Subscribe(Action<object, T> action);
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