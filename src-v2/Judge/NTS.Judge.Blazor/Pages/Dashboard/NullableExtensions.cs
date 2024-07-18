namespace NTS.Judge.Blazor.Pages.Dashboard;

public static class NullableExtensions
{
    public const string DEFAULT = "-";

    public static string OrDefault(this object? obj)
    {
        return obj != null
            ? obj.ToString() ?? throw new Exception($"Type '{obj.GetType()}' returns null from ToString")
            : DEFAULT;
    }
}
