using Not.DateAndTime;

namespace NTS.Domain;

public record Timestamp : DomainObject
{
    public Timestamp()
    {
        DateTime = DateTimeHelper.Now;
    }
    private Timestamp(DateTimeOffset dateTime)
    {
        DateTime = dateTime;
    }

    public DateTimeOffset DateTime { get; }

    public Timestamp Add(TimeSpan span)
    {
        return new Timestamp(DateTime.Add(span));
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        return this.DateTime.LocalDateTime.ToString(format, formatProvider);
    }
    public override string ToString()
    {
        return this.DateTime.LocalDateTime.ToString("HH:mm:ss.fff");
    }

    public static bool operator <(Timestamp? left, Timestamp? right)
    {
        return left?.DateTime < right?.DateTime;
    }
    public static bool operator >(Timestamp? left, Timestamp? right)
    {
        return left?.DateTime > right?.DateTime;
    }

    public static bool operator <(Timestamp? left, DateTimeOffset? right)
    {
        return left?.DateTime < right;
    }
    public static bool operator >(Timestamp? left, DateTimeOffset? right)
    {
        return left?.DateTime > right;
    }
    public static TimeSpan operator -(Timestamp? left, Timestamp? right)
    {
        return left?.DateTime - right?.DateTime ?? TimeSpan.Zero;
    }
    public static Timestamp? operator +(Timestamp? left, TimeSpan? right)
    {
        return left == null
            ? null
            : new Timestamp(left!.DateTime + (right ?? TimeSpan.Zero));
    }
}
