namespace NTS.Domain.Objects;

public record TimeInterval : DomainObject
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


    public double ToTotalHours()
    {
        return Span.TotalHours; 
    }
    public override string ToString()
    {
        return Span.ToString(Span.TotalDays > 1 ? @"dd\.hh\:mm\:ss" : @"hh\:mm\:ss");
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
        
    public static Speed? operator /(double num, TimeInterval? interval)
    {
        if (interval == null)
        {
            return null;
        }
        return new Speed(num, interval.ToTotalHours());
    }
}
