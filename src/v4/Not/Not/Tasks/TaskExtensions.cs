namespace Not.Tasks;

public static class TaskExtensions
{
	public static void AddTo(this Task task, List<Task> tasks)
	{
		tasks.Add(task);
	}
}
