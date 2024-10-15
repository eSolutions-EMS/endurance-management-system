using Not.Injection;
using Not.Safe;

namespace Not.Events;

// Redesign EventManager:
// - use custom delegate and maybe extensions methods
// - events should be instances of deleage attached at appropraite places
// - should not have to instantiate a new EventManager to fire or subscribe to event
public class EventManager : IEventManager
{
    readonly Dictionary<Guid, NotEventHandler> _handlersByGuid = [];
    event NotEventHandler? _delegate;

    public void Emit()
    {
        _delegate?.Invoke();
    }

    public Guid Subscribe(Func<Task> action)
    {
        return InternalSubscribe(() => SafeHelper.RunAsync(() => action()));
    }

    public Guid Subscribe(Action action)
    {
        return InternalSubscribe(() => SafeHelper.RunAsync(() => { action(); return Task.CompletedTask; }));
    }

    public void Unsubscribe(Guid guid) 
    {
        if (!_handlersByGuid.TryGetValue(guid, out var subscription))
        {
            return;
        }
        _delegate -= subscription;
    }

    Guid InternalSubscribe(NotEventHandler handler)
    {
        var guid = Guid.NewGuid();
        _handlersByGuid.Add(guid, handler);
        _delegate += handler;
        return guid;
    }
}

public class SyncEventManager
{
    event NotEventHandler? NotDelegate;
    readonly Dictionary<Guid, NotEventHandler> _actionSubscriptions = [];

    public void Emit()
    {
        NotDelegate?.Invoke();
    }
    public void Subscribe(Func<Task> action)
    {
        NotDelegate += () => SafeHelper.RunAsync(() => action());
    }
    public Guid Subscribe(Action action)
    {
        var guid = Guid.NewGuid();
        NotEventHandler safeAction = async () => await SafeHelper.Run(() => { action(); return Task.CompletedTask; });
        _actionSubscriptions.Add(guid, safeAction);
        NotDelegate += safeAction;
        return guid;
    }
    public void Unsubscribe(Guid guid)
    {
        if (!_actionSubscriptions.TryGetValue(guid, out var subscription))
        {
            return;
        }
        NotDelegate -= subscription;
    }
}

public class EventManager<T> : IEventManager<T>
    where T : IEvent
{
    readonly Dictionary<Guid, NotHandler<T>> _handlersByGuid = [];
    event NotHandler<T>? _delegate;

    public void Emit(T data)
    {
        _delegate?.Invoke(data);
    }

    public Guid Subscribe(Func<T, Task> action)
    {
        return InternalSubscribe(x => SafeHelper.RunAsync(() => action(x)));
    }

    public Guid Subscribe(Action<T> action)
    {
        return InternalSubscribe(x => SafeHelper.RunAsync(() => { action(x); return Task.CompletedTask; }));
    }

    public void Unsubscribe(Guid guid)
    {
        if (!_handlersByGuid.TryGetValue(guid, out var subscription))
        {
            return;
        }
        _delegate -= subscription;
    }

    Guid InternalSubscribe(NotHandler<T> handler)
    {
        var guid = Guid.NewGuid();
        _handlersByGuid.Add(guid, handler);
        _delegate += handler;
        return guid;
    }
}

public interface IEventManager : ITransientService
{
    void Emit();
    Guid Subscribe(Func<Task> action);
    Guid Subscribe(Action action);
    void Unsubscribe(Guid guid);
}

public interface IEventManager<T> : ITransientService
    where T : IEvent
{
    void Emit(T data);
    Guid Subscribe(Func<T, Task> action);
    Guid Subscribe(Action<T> action);
}