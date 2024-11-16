using Not.Localization;
using NTS.Domain.Core.Entities;
using NTS.Domain.Extensions;
using static NTS.Domain.Extensions.DomainExtensions;

namespace NTS.Domain.Core.Objects;

public record Start : DomainObject
{
    public Start(
        Person athlete,
        int number,
        int loopNumber,
        double distance,
        DateTime startAt
    )
    {
        Athlete = athlete;
        Number = number;
        PhaseNumber = loopNumber;
        Distance = distance;
        Time = startAt;
    }

    public Start(Participation participation)
    {
        Athlete = participation.Combination.Name;
        Number = participation.Combination.Number;
        var currentIndex = participation.Phases.IndexOf(participation.Phases.Current);
        var nextPhase = participation.Phases[currentIndex + 1];
        PhaseNumber = participation.Phases.NumberOf(nextPhase);
        Distance = nextPhase.Length;
        Time = nextPhase.StartTime!.DateTime;
    }

    public Person Athlete { get; private set; }
    public int Number { get; private set; }
    public int PhaseNumber { get; private set; }
    public double Distance { get; private set; }
    public DateTimeOffset Time { get; private set; }

    public override string ToString()
    {
        var distance = Distance + "km".Localize();
        var result = Combine(Number, Athlete, distance, Time.TimeOfDay);
        return result;
    }
}
