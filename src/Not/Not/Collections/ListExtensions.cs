namespace Not.Collections;

public static class ListExtensions
{
    public static void RemoveIfExisting<T>(this IList<T> list, Func<T, bool> predicate)
    {
        var existing = list.FirstOrDefault(predicate);
        if (existing != null)
        {
            list.Remove(existing);
        }
    }
}
