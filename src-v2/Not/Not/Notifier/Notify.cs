using Not.Events;
using Not.Injection;
using Not.Notifier;

namespace Not.Ports;

public class EventNotify : INotify
{
    public void Failed(string message)
    {
        EventHelper.Emit(new Failed(message));
    }

    public void Informed(string message)
    {
        EventHelper.Emit(new Informed(message));
    }

    public void Succeeded(string message)
    {
        EventHelper.Emit(new Succeeded(message));
    }

    public void Warned(string message)
    {
        EventHelper.Emit(new Warned(message));
    }
}

public interface INotify : ITransientService
{
    void Informed(string message);
    void Succeeded(string message);
    void Warned(string message);
    void Failed(string message);
}
