namespace NTS.Domain.Objects;

public record Speed : DomainObject
{
    public static Speed? Create(double? speed)
    {
        return speed.HasValue ? new Speed(speed.Value) : null;
    }

    public static implicit operator double(Speed speed)
    {
        return speed._speed;
    }

    public static bool operator >(Speed? a, Speed? b)
    {
        return a?._speed > b?._speed;
    }

    public static bool operator <(Speed? a, Speed? b)
    {
        return a?._speed < b?._speed;
    }

    Speed() { }

    Speed(double value)
    {
        _speed = value;
    }

    public Speed(double distance, double totalHours)
    {
        _speed = distance / totalHours;
    }

    public Speed(double distance, TimeInterval interval)
    {
        _speed = distance / interval.ToTotalHours();
    }

    // TODO: implement serialization of private fields and change to field
    public double _speed { get; private set; }

    public override string ToString()
    {
        return $"{_speed:0.00}";
    }
}
