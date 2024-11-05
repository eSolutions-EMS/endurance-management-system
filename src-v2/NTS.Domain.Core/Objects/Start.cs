using NTS.Domain.Core.Entities.ParticipationAggregate;
using NTS.Domain;
using NTS.Domain.Core.Entities;
using Not.Localization;

namespace NTS.Domain.Core.Objects;

public class StartList
{
    public StartList(IEnumerable<Participation> participations)
    {
        foreach (var participation in participations)
        {
            if (!participation.IsEliminated())
            {
                var phases = participation.Phases;
                var now = new Timestamp(DateTime.Now);
                foreach (var phase in phases.Where(p=>p.StartTime!=null))
                {
                    var index = phases.IndexOf(phase);
                    if (now - phase.StartTime > TimeSpan.FromMinutes(15))
                    {
                        var previousStart = new Start(participation.Combination.Name, participation.Combination.Number, index + 1, phases[index].Length, participation.Phases.Distance, phase.StartTime!);
                        PreviousStarts.Add(previousStart);
                    }
                    else
                    {
                        var upcomingStart = new Start(participation.Combination.Name, participation.Combination.Number, index + 1, phases[index].Length, participation.Phases.Distance, phase.StartTime!);
                        UpcomingStarts.Add(upcomingStart);
                    }
                }
            }
        }
    }
    public List<Start> UpcomingStarts { get; set; } = new List<Start>();
    public List<Start> PreviousStarts { get; set; } = new List<Start>();
}
public class Start
{
    public Start(Person athlete, int number, int loopNumber, double distance, double totalDistance, Timestamp startAt)
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
        if(StartAt == null) return string.Empty;
        if (StartAt > Timestamp.Now())
        {
            return (StartAt - Timestamp.Now()).ToString();
        }
        else
        {
            return "-" + (StartAt - Timestamp.Now()).ToString();
        }
    }

    public override string ToString()
    {
        var result = $"{Number}, {Athlete}, {"#".Localize()}{PhaseNumber}: {Distance}km, {StartAt}";
        return result;
    }
}