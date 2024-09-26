using Not.Exceptions;
using Not.Notifier;
using System.Diagnostics;
using System.Text;

namespace Not.Safe;

public static class SafeHelper
{
    public static Task RunAsync(Func<Task> action)
    {
        return Task.Run(() => Run(action));
    }

    public static async Task Run(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (DomainExceptionBase validation)
        {
            Notify.Warn(validation);
        }
        catch (Exception ex)
        {
            HandleError(ex);
        }
    }

    public static Task RunAsync<T>(Func<T, Task> action, T argument)
    {
        return Task.Run(() => Run(action, argument));
    }

    public static async Task Run<T>(Func<T, Task> action, T argument)
    {
        try
        {
            await action(argument);
        }

        catch (DomainExceptionBase validation)
        {
            Notify.Warn(validation);
        }
        catch (Exception ex)
        {
            HandleError(ex);
        }
    }

    static void HandleError(Exception ex)
    {
        Notify.Error(ex);
        WriteToTraceConsole(ex);
    }

    static void WriteToTraceConsole(Exception exception)
    {
        // TODO: add notification
        var sb = new StringBuilder();
        sb.AppendLine("!!!!!!!!!!!!!!!!!!!!!!!!! TASKHELPER EXCEPTION START !!!!!!!!!!!!!!!!!!!!!!!!!");
        sb.AppendLine("!!!!!!!!!!!!!!!!!!!!!!!!! TASKHELPER EXCEPTION START !!!!!!!!!!!!!!!!!!!!!!!!!");
        sb.AppendLine("!!!!!!!!!!!!!!!!!!!!!!!!! TASKHELPER EXCEPTION START !!!!!!!!!!!!!!!!!!!!!!!!!");
        sb.AppendLine("!!!!!!!!!!!!!!!!!!!!!!!!! TASKHELPER EXCEPTION START !!!!!!!!!!!!!!!!!!!!!!!!!");
        sb.AppendLine(exception.Message);
        sb.AppendLine(exception.StackTrace);
        sb.AppendLine("!!!!!!!!!!!!!!!!!!!!!!!!!! TASKHELPER EXCEPTION END !!!!!!!!!!!!!!!!!!!!!!!!!!");

        Trace.WriteLine(sb.ToString(), "console");
    }
}
