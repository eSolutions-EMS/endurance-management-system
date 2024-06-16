using Newtonsoft.Json;

namespace NTS.Domain.Setup.Entities;
public class Loop : DomainEntity
{
    public static Loop Create(double distance) => new (distance);
    public static Loop Update(int id, double distance) => new(id, distance);

    [JsonConstructor]
    public Loop(int id, double distance) : this(distance)
    {
        Id = id;
    }
    public Loop(double distance)
    {
        if (distance <= 0)
        {
            throw new DomainException(nameof(Distance), "Distance cannot be zero or less.");
        }

        Distance = distance;
    }
    public double Distance { get; set; }

    public override string ToString() 
    {
        var unit = "km".Localize();
        return $"{Distance}{unit}";
    }
}
