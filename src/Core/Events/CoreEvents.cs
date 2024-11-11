using System;

namespace Core.Events;

public static class CoreEvents
{
    public static event EventHandler<Exception> ErrorEvent;

    public static void RaiseError(Exception exception)
    {
        ErrorEvent?.Invoke(null, exception);
    }

    public static event EventHandler StateLoadedEvent;

    public static void RaiseStateLoaded()
    {
        StateLoadedEvent?.Invoke(null, EventArgs.Empty);
    }
}
