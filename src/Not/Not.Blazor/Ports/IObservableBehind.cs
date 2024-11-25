using Not.Injection;

namespace Not.Blazor.Ports;

public interface IObservableBehind : ISingleton
{
    Task Initialize(params IEnumerable<object> arguments);
    void Subscribe(Func<Task> action);
}
