namespace Not.Concurrency;

// TODO: This should be in its own namespace. Something like Not.Async or Not.Tasks
public static class TaskExtensions
{
    public static void ToVoid(this Task task)
    {
        task.ContinueWith(x => { });
    }

    public static void ToVoid<T>(this Task<T> task)
    {
        task.ContinueWith(x => { });
    }

    public static async Task<List<T>> ToList<T>(this Task<IEnumerable<T>> enumerableTask)
    {
        return (await enumerableTask).ToList();
    }
}
