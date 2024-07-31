using Newtonsoft.Json;
using Not.Localization;

namespace NTS.Domain.Core.Aggregates.Participations;

public class Tandem : DomainEntity
{
    private decimal _distance;

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
        MinAverageSpeed = minAverageSpeedlimit;
        MaxAverageSpeed = maxAverageSpeedLimit;
    }

    public int Number { get; private set; }
    public Person Name { get; private set; }
    public string Horse { get; private set; }
    public Country? Country { get; private set; }
    public Club? Club { get; private set; }
    public double? MinAverageSpeed { get; private set; }
    public double? MaxAverageSpeed { get; private set; }
    public string Distance
    { 
        get => _distance.ToString("#.##");
        set => _distance = decimal.Parse(value);
    }

    public override string ToString()
    {
        return $"{"#".Localize()}{Number}: {Name}, {Horse}, {Distance} {"км".Localize()}";
    }
}
