using Not.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Not.Extensions;
public static class DateTimeOffsetExtension
{
    public static DateTimeOffset ToDateTimeOffset(this DateTime time)
    {
        DateTime timeWithSpecifiedKind = DateTime.SpecifyKind(time, DateTimeKind.Local);
        DateTimeOffset offsetTime = timeWithSpecifiedKind;
        return offsetTime;
    }
    public static DateTimeOffset ToDateTimeOffset(this TimeSpan timeToBeAdded)
    {
        DateTime today = DateTime.Today;
        DateTime time = today.Add(timeToBeAdded);
        DateTime timeWithSpecifiedKind = DateTime.SpecifyKind(time, DateTimeKind.Local);
        DateTimeOffset offsetTime = timeWithSpecifiedKind;
        return offsetTime;
    }
}
