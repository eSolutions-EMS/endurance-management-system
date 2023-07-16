namespace Core.Extensions;

public static class StringExtensions
{
    public static int CompareTo(this string a, string b)
    {
        return string.Compare(a, b);
    }
}
