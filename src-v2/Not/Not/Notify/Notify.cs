﻿using Not.Events;
using Not.Exceptions;
using static Not.Notifier.Notifications;

namespace Not.Notifier;

public static class Notifications
{
    public readonly static EventManager<Informed> InformedEvent = new();
    public readonly static EventManager<Succeeded> SucceededEvent = new();
    public readonly static EventManager<Warned> WarnedEvent = new();
    public readonly static EventManager<Failed> FailedEvent = new();
}

public static class NotifyHelper
{
    public static void Inform(string message)
    {
        InformedEvent.Emit(new Informed(message));
    }

    public static void Success(string message)
    {
        SucceededEvent.Emit(new Succeeded(message));
    }

    public static void Warn(string message)
    {
        WarnedEvent.Emit(new Warned(message));
    }

    public static void Warn(DomainExceptionBase validation)
    {
        Warn(validation.Message);
    }

    public static void Error(Exception exception)
    {
        FailedEvent.Emit(new Failed(exception.Message + Environment.NewLine + exception.StackTrace));
    }
}
