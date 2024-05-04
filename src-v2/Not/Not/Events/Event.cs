using Not.Injection;

namespace Not.Events;

// TODO: how to unsubscribe? Maybe use Caller* attributes to keep callback references in a dictionary?
public class Event : IEvent
{
    private event NotEventHandler? NotDelegate;

    public void Emit()
    {
        NotDelegate?.Invoke();
    }
    public void Subscribe(Action action)
    {
        NotDelegate += () => action();
    }
}

public class Event<T> : IEvent<T>
    where T : class
{
    private event NotHandler<T>? GenericEvent;

    public void Emit(T data)
    {
        GenericEvent?.Invoke(data);
    }
    public void Subscribe(Action<T> action)
    {
        GenericEvent += x => action(x);
    }
}

public interface IEvent : ISingletonService
{
    void Emit();
    void Subscribe(Action action);
}

public interface IEvent<T> : ISingletonService
    where T : class
{
    void Emit(T data);
    void Subscribe(Action<T> action);
}