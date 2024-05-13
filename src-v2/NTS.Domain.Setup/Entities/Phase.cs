using Newtonsoft.Json;

namespace NTS.Domain.Setup.Entities;
public class Phase : DomainEntity
{
    public static Phase Create(double distance) => new (distance);
    public static Phase Update(int id, double distance) => new(id, distance);

    [JsonConstructor]
    public Phase(int id, double distance) : this(distance)
    {
        Id = id;
    }
    public Phase(double distance)
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
        var phase = "Phase".Localize();
        var sb = new StringBuilder();
        sb.Append($"{phase} -> {Distance}km long ");
        return sb.ToString();
    }
}
