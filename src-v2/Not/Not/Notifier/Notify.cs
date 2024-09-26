using Not.Events;

namespace Not.Notifier;

public static class Notify
{
    public static void Failed(string message)
    {
        EventHelper.Emit(new Failed(message));
    }

    public static void Informed(string message)
    {
        EventHelper.Emit(new Informed(message));
    }

    public static void Succeeded(string message)
    {
        EventHelper.Emit(new Succeeded(message));
    }

    public static void Warned(string message)
    {
        EventHelper.Emit(new Warned(message));
    }
}
