using Newtonsoft.Json;

namespace NTS.Domain.Setup.Entities;
public class Phase : DomainEntity
{
    [JsonConstructor]
    public Phase(double distance)
    {
        if (distance <= 0)
        {
            throw new DomainException(nameof(Distance), "Distance cannot be zero or less.");
        }

        Distance = distance;
    }
    public double Distance { get; set; }
}
