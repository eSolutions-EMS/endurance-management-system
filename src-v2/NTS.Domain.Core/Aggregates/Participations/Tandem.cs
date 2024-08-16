using Not.Localization;

namespace NTS.Domain.Core.Aggregates.Participations;

// TODO: probably shoudl be a record
public class Tandem : DomainEntity
{
    private decimal _distance;

    private Tandem(int id) : base(id)
    {
    }
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
        MinAverageSpeed = Speed.Create(minAverageSpeedlimit);
        MaxAverageSpeed = Speed.Create(maxAverageSpeedLimit);
    }

    public int Number { get; private set; }
    public Person Name { get; private set; }
    public string Horse { get; private set; }
    public Country? Country { get; private set; }
    public Club? Club { get; private set; }
    public Speed? MinAverageSpeed { get; private set; }
    public Speed? MaxAverageSpeed { get; private set; }
    public string Distance
    { 
        get => _distance.ToString("#.##");
        set => _distance = decimal.Parse(value);
    }

    public override string ToString()
    {
        var message = $"{"#".Localize()}{Number}: {Name}, {Horse}";
        var kmph = "km/h".Localize();
        if (MinAverageSpeed != null && MaxAverageSpeed != null)
        {
            return message + $" ({MinAverageSpeed}-{MaxAverageSpeed} {kmph})";
        }
        else if (MinAverageSpeed != null)
        {
            return message + $" ({"min".Localize()}:{MinAverageSpeed} {kmph})";
        }
        else
        {
            return message + $" ({"max".Localize()} : {MaxAverageSpeed}   {kmph})";
        }
    }
}
