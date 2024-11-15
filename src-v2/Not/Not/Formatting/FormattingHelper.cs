namespace Not.Formatting;

public static class FormattingHelper
{
    public static string TimeSpanFormat(TimeSpan timeSpan)
    {
        return $"{timeSpan:hh\\:mm\\:ss}";
    }
}
