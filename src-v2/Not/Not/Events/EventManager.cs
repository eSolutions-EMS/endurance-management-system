using Not.Injection;
using Not.Safe;

namespace Not.Events;

// TODO: how to unsubscribe? Maybe use Caller* attributes to keep callback references in a dictionary?
public class EventManager : IEventManager
{
    private static event NotEventHandler? NotDelegate;

    public void Emit()
    {
        NotDelegate?.Invoke();
    }
    public void Subscribe(Func<Task> action)
    {
        NotDelegate += () => SafeHelper.RunAsync(() => action());
    }
    public void Subscribe(Action action)
    {
        NotDelegate += () => SafeHelper.RunAsync(() => { action(); return Task.CompletedTask; });
    }
}

public class EventManager<T> : IEventManager<T>
    where T : IEvent
{
    private static event NotHandler<T>? GenericEvent;

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
    void Subscribe(Action action);
}

public interface IEventManager<T> : ITransientService
    where T : IEvent
{
    void Emit(T data);
    void Subscribe(Func<T, Task> action);
    void Subscribe(Action<T> action);
}