namespace NTS.Domain.Objects;

public record TimeInterval
{
    public static TimeInterval Zero => new(TimeSpan.Zero);

    private TimeInterval()
    {
    }
    public TimeInterval(TimeSpan timeSpan)
    {
        Span = timeSpan;
    }
    
    public TimeSpan Span { get; private set; }  

    public override string ToString()
    {
        return Span.ToString(@"dd\.hh\:mm\:ss");
    }

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

    public static double? operator /(double num, TimeInterval? interval)
    {
        return num / interval?.Span.TotalHours;
    }
}
