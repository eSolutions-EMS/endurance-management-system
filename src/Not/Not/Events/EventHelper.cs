namespace Not.Events;

public static class EventHelper
{
    public static void Emit<T>(T payload)
        where T : IEvent
    {
        new EventManager<T>().Emit(payload);
    }

    public static void Subscribe<T>(Action<T> callback)
        where T : IEvent
    {
        new EventManager<T>().Subscribe(callback);
    }
}
