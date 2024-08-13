namespace Not.Concurrency;

public static class TaskHelper
{
    public static Task Run(Action action)
    {
        try
        {
            return Task.Run(() => action());
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    public static Task Run<T>(Action<T> action, T argument)
    {
        try
        {
            return Task.Run(() => action(argument));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    static Task HandleError(Exception exception)
    {
        // TODO: add notification
        throw new NotImplementedException($"Notification for error '{exception.Message}'");
    }
}
