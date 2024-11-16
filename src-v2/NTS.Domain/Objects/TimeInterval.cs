using Not.Domain.Base;

namespace NTS.Domain.Objects;

public record TimeInterval : DomainObject, IComparable<TimeInterval>
{
    public static implicit operator TimeSpan?(TimeInterval? interval)
    {
        return interval?.Span;
    }

    public static TimeInterval? operator +(TimeInterval? one, TimeInterval? two)
    {
        if (one == null || two == null)
        {
            return null;
        }
        return new TimeInterval(one!.Span + two!.Span);
    }

    public static TimeInterval? operator -(TimeInterval? one, TimeInterval? two)
    {
        if (one == null || two == null)
        {
            return null;
        }
        return new TimeInterval(one!.Span - two!.Span);
    }

    public static Speed? operator /(double num, TimeInterval? interval)
    {
        if (interval == null)
        {
            return null;
        }
        return new Speed(num, interval.ToTotalHours());
    }

    TimeInterval() { }

    public TimeInterval(TimeSpan timeSpan)
    {
        Span = timeSpan;
    }

    public static TimeInterval Zero => new(TimeSpan.Zero);
    public TimeSpan Span { get; private set; }

    public double ToTotalHours()
    {
        return Span.TotalHours;
    }

    public override string ToString()
    {
        // TODO: use FormattingHelper from Startlist PR
        return Span.ToString(Span.TotalDays > 1 ? @"dd\.hh\:mm\:ss" : @"hh\:mm\:ss");
    }

    public int CompareTo(TimeInterval? other)
    {
        return Span.CompareTo(other?.Span);
    }
}
