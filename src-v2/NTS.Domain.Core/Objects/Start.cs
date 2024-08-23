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
    }

    public int Number { get; private set; }
    public Person Athlete { get; private set; }
    public int LoopNumber { get; private set; }
    public double Distance { get; private set; }
    public Phase CurrentPhase { get; private set; }
    public Timestamp StartAt { get; private set; }
    public TimeInterval StartIn => StartAt - Timestamp.Now();

    public override string ToString()
    {
        var result = $"{Number}, {Athlete}, {LoopNumber}, {Distance}, {StartAt}, {StartIn}";
        return result;
    }
}
