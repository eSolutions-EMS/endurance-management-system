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
                foreach (var phase in phases)
                {
                    var index = phases.IndexOf(phase);
                    if (phase.IsComplete())
                    {
                        var previousStart = new Start(participation.Combination.Name, participation.Combination.Number, index + 1, phases[index].Length, participation.Phases.Distance, phase.StartTime!);
                        PreviousStarts.Add(previousStart);
                    }
                    else
                    {
                        if(phase == phases.Current && now - phase.StartTime < TimeSpan.FromMinutes(15))
                        {
                            var upcomingStart = new Start(participation.Combination.Name, participation.Combination.Number, index, phases[index].Length, participation.Phases.Distance, phase.StartTime!);
                            UpcomingStarts.Add(upcomingStart);
                        }
                    }
                }
            }
        }
    }
    public List<Start> UpcomingStarts { get; set; } = new List<Start>();
    public List<Start> PreviousStarts { get; set; } = new List<Start>();

    public IEnumerable<Start> GroupByDistanceAndAthlete()
    {
        var group1 = PreviousStarts
            .GroupBy(sh => Tuple.Create(sh.TotalDistance, sh.Athlete));
        var sort1 = group1
            .OrderBy(g => g.Key.Item1);
        var sort2 = sort1
            .ThenBy(g => g.Key.Item2); //returns IOrderedEnumerable<IGrouping<Tuple<double, Person>, Start>>
        var group = sort2
            .FlattenGroupedItems();
        return group;
    }
}
public class Start : IComparable<Start>
{
    public Start(Participation participation)
    {
        var phases = participation.Phases;
        Athlete = participation.Combination.Name;
        Number = participation.Combination.Number;
        PhaseNumber = phases.IndexOf(phases.Current)+1;
        Distance = participation.Phases.Current.Length;
        StartAt = participation.Phases.Current.StartTime;
        GuardHelper.ThrowIfDefault(participation?.Phases?.Current);
        //CurrentPhase = participation?.Phases?.Current;
        TotalDistance = participation!.Phases!.Distance;
    }

    public Start(Person athlete, int number, int loopNumber, double distance, double totalDistance, Timestamp startAt)
    {
        Athlete = athlete;
        Number = number;
        PhaseNumber = loopNumber;
        Distance = distance;
        TotalDistance = totalDistance;
        //CurrentPhase = phase;
        StartAt = startAt;
    }

    public Person Athlete { get; private set; }
    public int Number { get; private set; }
    public int PhaseNumber { get; private set; }
    public double Distance { get; private set; }
    public double TotalDistance { get; private set; }
    //public Phase CurrentPhase { get; private set; }
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
        //test cases -remove before merge
        //return "00:05:02";
        //return "00:00:03";
        //return "-00:14:57";
    }

    public override string ToString()
    {
        var result = $"{Number}, {Athlete}, {"#".Localize()}{PhaseNumber}: {Distance}km, {StartAt}";
        return result;
    }

    public int CompareTo(Start other)
    {
        if (other == null) return 1;

        // First compare Athlete
        int athleteComparison = string.Compare(Athlete, other.Athlete, StringComparison.Ordinal);
        if (athleteComparison != 0) return athleteComparison;

        // Then compare Competition Distance if Athlete is the same
        return string.Compare(TotalDistance.ToString(), other.TotalDistance.ToString(), StringComparison.Ordinal);
    }
}