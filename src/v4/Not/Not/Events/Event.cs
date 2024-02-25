using Common.Conventions;
using Common.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace Common.Events;

public class Event : IEvent
{
    private event EventHandler? GenericEvent;

    public void Emit(object sender)
    {
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