using Not.Concurrency;
using Not.Injection;

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
        NotDelegate += () => TaskHelper.Run(() => action());
    }
    public void Subscribe(Action action)
    {
        NotDelegate += () => TaskHelper.Run(() => { action(); return Task.CompletedTask; });
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
        GenericEvent += x => TaskHelper.Run(() => action(x));
    }
    public void Subscribe(Action<T> action)
    {
        GenericEvent += x => TaskHelper.Run(() => { action(x); return Task.CompletedTask; });
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