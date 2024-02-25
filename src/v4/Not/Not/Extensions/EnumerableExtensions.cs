namespace Not.Extensions;

public static class EnumerableExtensions
{
    public static T[] AsArray<T>(this T obj)
    {
        return new[] { obj };
    }
}
