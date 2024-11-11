using Not.Localization;

namespace NTS.Domain.Core.Objects;

public record Start : DomainObject
{
    public Start(
        Person athlete,
        int number,
        int loopNumber,
        double distance,
        double totalDistance,
        Timestamp startAt
    )
    {
        Athlete = athlete;
        Number = number;
        PhaseNumber = loopNumber;
        Distance = distance;
        TotalDistance = totalDistance.FloorWholeNumberToTens();
        StartAt = startAt;
    }

    public Person Athlete { get; private set; }
    public int Number { get; private set; }
    public int PhaseNumber { get; private set; }
    public double Distance { get; private set; }
    public double TotalDistance { get; private set; }
    public Timestamp? StartAt { get; private set; }

    public string StartIn()
    {
        if (StartAt == null) { return string.Empty; }
        if (StartAt > Timestamp.Now())
        {
            return (StartAt - Timestamp.Now())!.ToString();
        }
        else
        {
            return "-" + (StartAt - Timestamp.Now())!.ToString();
        }
    }

    public override string ToString()
    {
        var distance = Distance + "km".Localize();
        var result = Combine(Number, Athlete, distance, StartAt);
        return result;
    }
}
