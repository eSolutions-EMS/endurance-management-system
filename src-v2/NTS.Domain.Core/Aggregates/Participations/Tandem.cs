using Newtonsoft.Json;

namespace NTS.Domain.Core.Aggregates.Participations;

public class Tandem : DomainEntity
{
    private readonly decimal _distance;

    [JsonConstructor]
    public Tandem(
        int number,
        Person name,
        string horse,
        decimal distance,
        Country? country,
        Club? club,
        double? minAverageSpeedlimit,
        double? maxAverageSpeedLimit)
    {
        Number = number;
        Name = name;
        Horse = horse;
        _distance = distance;
        Country = country;
        Club = club;
        MinAverageSpeedlimit = minAverageSpeedlimit;
        MaxAverageSpeedLimit = maxAverageSpeedLimit;
    }

    public int Number { get; }
    public Person Name { get; }
    public string Horse { get; }
    public Country? Country { get; }
    public Club? Club { get; }
    public double? MinAverageSpeedlimit { get; }
    public double? MaxAverageSpeedLimit { get; }
    public string Distance => _distance.ToString("0.00");
}
