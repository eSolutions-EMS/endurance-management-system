using Not.Exceptions;
using static Not.Notify.NotificationEvents;

namespace Not.Notify;

public static class NotifyHelper
{
    public static void Inform(string message)
    {
        NotificationEvents.Informed.Emit(new Information(message));
    }

    public static void Success(string message)
    {
        Succeded.Emit(new Success(message));
    }

    public static void Warn(string message)
    {
        NotificationEvents.Warned.Emit(new Warning(message));
    }

    public static void Warn(DomainExceptionBase validation)
    {
        Warn(validation.Message);
    }

    public static void Error(Exception exception)
    {
        NotificationEvents.Failed.Emit(
            new Failure(exception.Message + Environment.NewLine + exception.StackTrace)
        );
    }
}
