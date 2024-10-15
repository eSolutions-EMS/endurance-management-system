using Not.Injection;
using Not.Safe;

namespace Not.Events;

// TODO: how to unsubscribe? Maybe use Caller* attributes to keep callback references in a dictionary?
// Redesign EventManager:
// - use custom delegate and maybe extensions methods
// - events should be instances of deleage attached at appropraite places
// - should not have to instantiate a new EventManager to fire or subscribe to event
public class EventManager : IEventManager
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
        NotEventHandler safeAction = () => SafeHelper.RunAsync(() => { action(); return Task.CompletedTask; });
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
    private event NotHandler<T>? GenericEvent;

    public void Emit(T data)
    {
        GenericEvent?.Invoke(data);
    }
    public void Subscribe(Func<T, Task> action)
    {
        GenericEvent += x => SafeHelper.RunAsync(() => action(x));
    }
    public void Subscribe(Action<T> action)
    {
        GenericEvent += x => SafeHelper.RunAsync(() => { action(x); return Task.CompletedTask; });
    }
}

public interface IEventManager : ITransientService
{
    void Emit();
    void Subscribe(Func<Task> action);
    Guid Subscribe(Action action);
    void Unsubscribe(Guid guid);
}

public interface IEventManager<T> : ITransientService
    where T : IEvent
{
    void Emit(T data);
    void Subscribe(Func<T, Task> action);
    void Subscribe(Action<T> action);
}