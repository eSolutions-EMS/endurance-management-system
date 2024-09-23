using NTS.Domain.Core.Aggregates.Participations;
using NTS.Domain;

namespace NTS.Domain.Core.Objects;

public class StartHistory
{
    public StartHistory(IEnumerable<Participation> participations)
    {
        foreach (var participation in participations)
        {
            foreach (var phase in participation.Phases)
            {
                var loopNumber = participation.Phases.NumberOf(phase ?? participation.Phases.Last());
                var previousStart = new Start(participation.Tandem.Name, participation.Tandem.Number, loopNumber, phase.Length, participation.Phases.Distance, phase, phase.OutTime);
                PreviousStarts.Add(previousStart);
            }
        }
    }
    public List<Start> PreviousStarts { get; private set; } = new List<Start>();
    public IEnumerable<IGrouping<double, Start>> StartlistByDistance => PreviousStarts.GroupBy(p => p.Distance);
}

public class StartList
{
    public StartList(IEnumerable<Participation> participations)
    {
        foreach(var participation in participations)
        {
            var phase = participation.Phases.Current;
            var loopNumber = participation.Phases.NumberOf(phase ?? participation.Phases.Last());
            var upcomingStart = new Start(participation.Tandem.Name, participation.Tandem.Number, loopNumber, phase.Length, participation.Phases.Distance, phase, phase.OutTime);
            UpcomingStarts.Add(upcomingStart);
        }
    }
    public Person Athlete { get; private set; }
    public List<Start> UpcomingStarts = new List<Start>();
    IEnumerable<IGrouping<double?, Start>> StartlistByDistance { get; }
}
public class Start
{
    public Start(Participation participation)
    {
        var phaseStart = new PhaseStart(participation);
        Athlete = participation.Tandem.Name;
        Number = participation.Tandem.Number;
        LoopNumber = phaseStart.LoopNumber;
        Distance = participation.Phases.Current.Length;
        StartAt = phaseStart.StartAt;
        GuardHelper.ThrowIfDefault(participation?.Phases?.Current);
        CurrentPhase = participation?.Phases?.Current;
        TotalDistance = participation?.Phases?.Distance;
    }

    public Start(Person athlete, int number, int loopNumber, double distance, double? totalDistance, Phase phase, Timestamp startAt)
    {
        Athlete = athlete;
        Number = number;
        LoopNumber = loopNumber;
        Distance = distance;
        TotalDistance = totalDistance;
        CurrentPhase = phase;
        StartAt = startAt;
    }

    public Person Athlete { get; private set; }
    public int Number { get; private set; }
    public int LoopNumber { get; private set; }
    public double Distance { get; private set; }
    public double? TotalDistance { get; private set; }
    public Phase CurrentPhase { get; private set; }
    public Timestamp StartAt { get; private set; }
    public string StartIn()
    {
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
        var result = $"{Number}, {Athlete}, #{LoopNumber}: {Distance}km, {StartAt}";
        return result;
    }
}
