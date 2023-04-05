using System;

namespace EnduranceJudge.Core.Events;

public static class CoreEvents
{
    public static event EventHandler<Exception> ErrorEvent;
    public static void RaiseError(Exception exception)
    {
        ErrorEvent?.Invoke(null, exception);
    }
}
