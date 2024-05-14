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
    public void Subscribe(Action action)
    {
        NotDelegate += () => action();
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
    public void Subscribe(Action<T> action)
    {
        GenericEvent += x => action(x);
    }
}

public interface IEventManager : ITransientService
{
    void Emit();
    void Subscribe(Action action);
}

public interface IEventManager<T> : ITransientService
    where T : IEvent
{
    void Emit(T data);
    void Subscribe(Action<T> action);
}