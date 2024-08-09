namespace NTS.Judge.Blazor.Pages.Dashboard;

public static class NullableExtensions
{
    public const string DEFAULT = "-";

    public static string OrDefault(this object? obj)
    {
        if (obj == null)
        {
            return DEFAULT;
        }
        var text = obj.ToString() ?? throw new Exception($"Type '{obj.GetType()}' returns null from ToString");
        return text;
    }
}
