using System.Globalization;
using Not.Domain.Base;

namespace NTS.Domain.Core.Objects;

public record EventSpan : DomainObject
{
    public EventSpan(DateTimeOffset startDay, DateTimeOffset endDay)
    {
        StartDay = startDay;
        EndDay = endDay;
    }

    public DateTimeOffset StartDay { get; }
    public DateTimeOffset EndDay { get; }

    public override string ToString()
    {
        var start = StartDay.ToLocalTime();
        var end = EndDay.ToLocalTime();
        var now = DateTimeOffset.UtcNow;
        DateTimeOffset date;
        if (now > start && now < end)
        {
            date = now;
        }
        else if (start > now)
        {
            date = start;
        }
        else
        {
            date = end;
        }
        return date.ToString("d", CultureInfo.CurrentCulture);
    }
}
