using System;
using Not.Localization;
using NTS.Domain.Core.Entities;

namespace NTS.Domain.Core.Objects;

public record Start : DomainObject
{
    public Start(
        Person athlete,
        int number,
        int loopNumber,
        double distance,
        double totalDistance,
        TimeSpan startAt
    )
    {
        Athlete = athlete;
        Number = number;
        PhaseNumber = loopNumber;
        Distance = distance;
        TotalDistance = totalDistance.RoundWholeNumberToTens();
        StartAt = startAt;
    }

    public Start(Participation participation)
    {
        Athlete = participation.Combination.Name;
        Number = participation.Combination.Number;
        var phaseIndex = participation.Phases.IndexOf(participation.Phases.Current);
        PhaseNumber = phaseIndex+2;
        Distance = participation.Phases[phaseIndex].Length;
        TotalDistance = participation.Phases.Distance;
        StartAt = participation.Phases[phaseIndex+1].StartTime!.DateTime.TimeOfDay;
    }

    public Person Athlete { get; private set; }
    public int Number { get; private set; }
    public int PhaseNumber { get; private set; }
    public double Distance { get; private set; }
    public double TotalDistance { get; private set; }
    public TimeSpan StartAt { get; private set; }

    public override string ToString()
    {
        var distance = Distance + "km".Localize();
        var result = Combine(Number, Athlete, distance, StartAt);
        return result;
    }
}
