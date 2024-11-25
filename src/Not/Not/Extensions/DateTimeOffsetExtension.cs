namespace Not.Extensions;

public static class DateTimeOffsetExtension
{
    public static DateTimeOffset ToDateTimeOffset(this DateTime time)
    {
        var timeWithSpecifiedKind = DateTime.SpecifyKind(time, DateTimeKind.Local);
        var offsetTime = timeWithSpecifiedKind;
        return offsetTime;
    }

    public static DateTimeOffset ToDateTimeOffset(this TimeSpan timeToBeAdded)
    {
        var today = DateTime.Today;
        var time = today.Add(timeToBeAdded);
        var timeWithSpecifiedKind = DateTime.SpecifyKind(time, DateTimeKind.Local);
        var offsetTime = timeWithSpecifiedKind;
        return offsetTime;
    }
}
