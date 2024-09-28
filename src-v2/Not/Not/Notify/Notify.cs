using Not.Events;
using Not.Exceptions;

namespace Not.Notifier;

public static class NotifyHelper
{
    public static void Inform(string message)
    {
        EventHelper.Emit(new Informed(message));
    }

    public static void Success(string message)
    {
        EventHelper.Emit(new Succeeded(message));
    }

    public static void Warn(string message)
    {
        EventHelper.Emit(new Warned(message));
    }

    public static void Warn(DomainExceptionBase validation)
    {
        Warn(validation.Message);
    }

    public static void Error(Exception exception)
    {
        EventHelper.Emit(new Failed(exception.Message + Environment.NewLine + exception.StackTrace));
    }
}
