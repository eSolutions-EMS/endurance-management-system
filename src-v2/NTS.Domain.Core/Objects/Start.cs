using NTS.Domain.Core.Aggregates.Participations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Domain.Core.Objects;
public class Start
{
    public Start(Participation participation)
    {
        var phaseStart = new PhaseStart(participation);
        Number = phaseStart.Number;
        Athlete = phaseStart.Athlete;
        LoopNumber = phaseStart.LoopNumber;
        Distance = phaseStart.Distance;
        StartAt = phaseStart.StartAt;
        GuardHelper.ThrowIfDefault(participation?.Phases?.Current);
        CurrentPhase = participation?.Phases?.Current;
        TotalDistance = participation?.Phases?.Distance;
    }

    public int Number { get; private set; }
    public Person Athlete { get; private set; }
    public int LoopNumber { get; private set; }
    public double Distance { get; private set; }
    public Phase CurrentPhase { get; private set; }
    public double? TotalDistance { get; private set; }
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
        //test cases - remove before merge      
        //return "00:05:02";
        //return "00:00:03";
        //return "-00:14:57";
    }

    public override string ToString()
    {
        var result = $"{Number}, {Athlete}, #{LoopNumber}: {Distance}km , {StartAt}";
        return result;
    }
}
