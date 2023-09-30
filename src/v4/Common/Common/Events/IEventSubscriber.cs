using Common.Conventions;

namespace Common.Events;

public interface IEventSubscriber : ITransientService
{
    void Subscribe(Action<object> action);
}

public interface IEventSubscriber<T> : ITransientService
{
    void Subscribe(Action<object, T> action);
}

public interface IEventSubscriber<T1, T2> : ITransientService
{
    void Subscribe(Action<object, T1, T2> action);
}
