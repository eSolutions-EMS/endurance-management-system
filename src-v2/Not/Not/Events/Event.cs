using Not.Safe;

namespace Not.Events;

// Redesign EventManager:
// - use custom delegate and maybe extensions methods
// - events should be instances of deleage attached at appropraite places
// - should not have to instantiate a new EventManager to fire or subscribe to event
public abstract class EventBase<T>
{
    readonly Dictionary<Guid, T> _handlersByGuid = [];

    protected abstract void AddHandler(T handler);
    protected abstract void RemoveHandler(T handler);

    protected Guid InternalSubscribe(T handler)
    {
        var guid = Guid.NewGuid();
        _handlersByGuid.Add(guid, handler);
        AddHandler(handler);
        return guid;
    }

    protected Task ReturnCompletedTask(Action action)
    {
        action();
        return Task.CompletedTask;
    }

    protected Task ReturnCompletedTask<TArgument>(Action<TArgument> action, TArgument argument)
    {
        action(argument);
        return Task.CompletedTask;
    }

    public void Unsubscribe(Guid guid)
    {
        if (!_handlersByGuid.TryGetValue(guid, out var handler))
        {
            return;
        }
        RemoveHandler(handler);
    }
}

public class Event : EventBase<NotEventHandler>, IEventSubscriber
{
    event NotEventHandler? _delegate;

    public void Emit()
    {
        _delegate?.Invoke();
    }

    public Guid Subscribe(Func<Task> action)
    {
        return InternalSubscribe(() => SafeHelper.Run(() => action()));
    }

    public Guid Subscribe(Action action)
    {
        return InternalSubscribe(() => SafeHelper.Run(() => ReturnCompletedTask(action)));
    }

    public Guid SubscribeAsync(Func<Task> action)
    {
        return InternalSubscribe(() => SafeHelper.RunAsync(() => action()));
    }

    public Guid SubscribeAsync(Action action)
    {
        return InternalSubscribe(() => SafeHelper.RunAsync(() => ReturnCompletedTask(action)));
    }

    protected override void AddHandler(NotEventHandler handler)
    {
        _delegate += handler;
    }

    protected override void RemoveHandler(NotEventHandler handler)
    {
        _delegate -= handler;
    }
}

public class Event<T> : EventBase<NotHandler<T>>, IEventManager<T>
{
    event NotHandler<T>? _delegate;

    public void Emit(T data)
    {
        _delegate?.Invoke(data);
    }

    public Guid Subscribe(Func<T, Task> action)
    {
        return InternalSubscribe(x => SafeHelper.Run(() => action(x)));
    }

    public Guid Subscribe(Action<T> action)
    {
        return InternalSubscribe(x => SafeHelper.Run(() => ReturnCompletedTask(action, x)));
    }

    public Guid SubscribeAsync(Func<T, Task> action)
    {
        return InternalSubscribe(x => SafeHelper.RunAsync(() => action(x)));
    }

    public Guid SubscribeAsync(Action<T> action)
    {
        return InternalSubscribe(x => SafeHelper.RunAsync(() => ReturnCompletedTask(action, x)));
    }

    protected override void AddHandler(NotHandler<T> handler)
    {
        _delegate += handler;
    }

    protected override void RemoveHandler(NotHandler<T> handler)
    {
        _delegate -= handler;
    }
}

public interface IEventSubscriber
{
    Guid Subscribe(Func<Task> action);
    Guid Subscribe(Action action);
    Guid SubscribeAsync(Func<Task> action);
    Guid SubscribeAsync(Action action);
    void Unsubscribe(Guid guid);
}

public interface IEventManager<T>
{
    Guid Subscribe(Func<T, Task> action);
    Guid Subscribe(Action<T> action);
    Guid SubscribeAsync(Func<T, Task> action);
    Guid SubscribeAsync(Action<T> action);
}