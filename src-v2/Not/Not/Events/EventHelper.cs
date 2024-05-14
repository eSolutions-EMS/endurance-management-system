namespace Not.Events;

public static class EventHelper
{
    public static void Emit<T>(T payload)
    {
        new EventManager<T>().Emit(payload);
    }

    public static void Subscribe<T>(Action<T> callback)
    {
        new EventManager<T>().Subscribe(callback);
    }
}
