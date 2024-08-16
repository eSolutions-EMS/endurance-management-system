using System.Diagnostics;
using System.Text;

namespace Not.Concurrency;

public static class TaskHelper
{
    public static Task Run(Func<Task> action)
    {
        return Task.Run(async () =>
        {
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        });
    }

    public static Task Run<T>(Func<T, Task> action, T argument)
    {
        return Task.Run(async () =>
        {
            try
            {
                await action(argument);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        });
    }

    static void HandleError(Exception exception)
    {
        // TODO: add notification
        var sb = new StringBuilder();
        sb.AppendLine("TASKHELPER EXCEPTION!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        sb.AppendLine("TASKHELPER EXCEPTION!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        sb.AppendLine("TASKHELPER EXCEPTION!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        sb.AppendLine("TASKHELPER EXCEPTION!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        sb.AppendLine(exception.Message);
        sb.AppendLine(exception.StackTrace);
        sb.AppendLine("TASKHELPER EXCEPTION!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

        Trace.WriteLine(sb.ToString(), "console");
    }
}
