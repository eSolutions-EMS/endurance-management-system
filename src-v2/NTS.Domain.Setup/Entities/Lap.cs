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

    public string NestedToString()
    {
        return Distance.ToString()+"km";
    }
    public override string ToString() 
    {
        var lap = "Lap".Localize();
        var message = $"{lap} -> {Distance}km long ";
        return message;
    }
}
