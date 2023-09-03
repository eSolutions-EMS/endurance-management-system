using System;
using System.Linq;

namespace Core.Utilities;

public static class DateUtilities
{
    public static DateTime Now => DateTime.Now;


    public static string FormatTime(DateTime time)
        => time.ToString("HH:mm:ss.fff");

    public static string FormatTime(TimeSpan time)
        => time.ToString(@"hh\:mm\:ss\.fff");

    public static string StripMilliseconds(this string formatted)
        => formatted.Split(".").First();
}
