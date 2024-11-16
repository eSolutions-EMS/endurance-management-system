using Newtonsoft.Json;
using Not.Domain.Base;

namespace NTS.Domain.Objects;

public record Timestamp : DomainObject, IComparable<Timestamp>
{
    public static Timestamp Now()
    {
        return new Timestamp(DateTimeOffset.Now);
    }

    public static Timestamp? Create(DateTimeOffset? dateTime)
    {
        if (dateTime == null)
        {
            return null;
        }
        return new Timestamp(dateTime.Value);
    }

    public static Timestamp Copy(Timestamp timestamp)
    {
        return new Timestamp(timestamp);
    }

    public static bool operator <(Timestamp? left, Timestamp? right)
    {
        return left?._stamp < right?._stamp;
    }

    public static bool operator >(Timestamp? left, Timestamp? right)
    {
        return left?._stamp > right?._stamp;
    }

    public static bool operator <=(Timestamp? left, Timestamp? right)
    {
        return left?._stamp <= right?._stamp;
    }

    public static bool operator >=(Timestamp? left, Timestamp? right)
    {
        return left?._stamp > right?._stamp;
    }

    public static bool operator <(Timestamp? left, DateTimeOffset? right)
    {
        return left?._stamp < right;
    }

    public static bool operator >(Timestamp? left, DateTimeOffset? right)
    {
        return left?._stamp > right;
    }

    public static TimeInterval? operator -(Timestamp? left, Timestamp? right)
    {
        if (left == null || right == null)
        {
            return null;
        }
        return new TimeInterval(left!._stamp - right!._stamp);
    }

    public static Timestamp? operator +(Timestamp? left, TimeSpan? right)
    {
        return left == null ? null : new Timestamp(left!._stamp + (right ?? TimeSpan.Zero));
    }

    protected Timestamp(Timestamp timestamp)
        : base(timestamp)
    {
        _stamp = timestamp._stamp;
    }

    public Timestamp(DateTime dateTime)
    {
        _stamp = dateTime;
    }

    public Timestamp(DateTimeOffset stamp)
    {
        _stamp = stamp;
    }

    [JsonProperty]
#pragma warning disable IDE1006 // TODO: serialize private setter using custom JsonConverter<Timestamp>
    public DateTimeOffset _stamp { get; set; }
#pragma warning restore IDE1006 // Naming Styles

    public Timestamp Add(TimeSpan span)
    {
        return new Timestamp(_stamp.Add(span));
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        return _stamp.LocalDateTime.ToString(format, formatProvider);
    }

    public override string ToString()
    {
        return _stamp.LocalDateTime.ToString("HH:mm:ss");
    }

    public int CompareTo(Timestamp? other)
    {
        return _stamp.CompareTo(other?._stamp ?? DateTimeOffset.MinValue);
    }

    public DateTimeOffset ToDateTimeOffset()
    {
        return _stamp; //TODO: test reference modification
    }

    public DateTime ToDateTime()
    {
        return _stamp.DateTime;
    }
}
