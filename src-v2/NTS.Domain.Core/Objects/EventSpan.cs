using System.Globalization;

namespace NTS.Domain.Core.Objects;

public record EventSpan : DomainObject
{
    private readonly DateTimeOffset _startDay;
    private readonly DateTimeOffset _endDay;

    public EventSpan(DateTimeOffset startDay, DateTimeOffset endDay)
    {
        _startDay = startDay;
        _endDay = endDay;
    }

    public override string ToString()
    {
        var start = _startDay.ToLocalTime();
        var end = _endDay.ToLocalTime();
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
        return date.ToString(CultureInfo.CurrentCulture);
    }
}
