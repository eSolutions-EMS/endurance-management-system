using System;
using System.Linq;

namespace Core.Utilities;

public static class DateUtilities
{
    public static DateTime Now => DateTime.Now;

    public static string FormatTime(DateTime time) => time.ToString("HH:mm:ss.fff");

    public static string FormatTime(TimeSpan time)
    {
        var formatted = time.ToString(@"hh\:mm\:ss\.fff");
        if (time.IsNegative())
        {
            return $"-{formatted}";
        }
        return formatted;
    }

    public static string StripMilliseconds(this string formatted) => formatted.Split(".").First();

    public static bool IsNegative(this TimeSpan span)
    {
        return span.TotalDays < 0;
    }
}
