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
    public static DateTimeOffset? ToDateTimeOffset(this TimeSpan? timeToBeAdded)
    {
        if(timeToBeAdded == null) return null;
        DateTime today = DateTime.Today;
        var time = today.Add((TimeSpan)timeToBeAdded);
        DateTime timeWithSpecifiedKind = DateTime.SpecifyKind(time, DateTimeKind.Local);
        DateTimeOffset offsetTime = timeWithSpecifiedKind;
        return offsetTime;
    }
}
