using Newtonsoft.Json;

namespace NTS.Domain.Setup.Entities;
public class Lap : DomainEntity
{
    public static Lap Create(double distance) => new (distance);
    public static Lap Update(int id, double distance) => new(id, distance);

    [JsonConstructor]
    public Lap(int id, double distance) : this(distance)
    {
        Id = id;
    }
    public Lap(double distance)
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
        var message = $"{Distance}km";
        return message;
    }
}
