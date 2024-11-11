using Not.Exceptions;
using static Not.Notify.NotificationEvents;

namespace Not.Notify;

public static class NotifyHelper
{
    public static void Inform(string message)
    {
        NotificationEvents.Informed.Emit(new Informed(message));
    }

    public static void Success(string message)
    {
        Succeded.Emit(new Succeeded(message));
    }

    public static void Warn(string message)
    {
        NotificationEvents.Warned.Emit(new Warned(message));
    }

    public static void Warn(DomainExceptionBase validation)
    {
        Warn(validation.Message);
    }

    public static void Error(Exception exception)
    {
        NotificationEvents.Failed.Emit(
            new Failed(exception.Message + Environment.NewLine + exception.StackTrace)
        );
    }
}
