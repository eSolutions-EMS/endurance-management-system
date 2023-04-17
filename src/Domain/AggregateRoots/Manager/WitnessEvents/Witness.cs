using EnduranceJudge.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using System;

namespace EnduranceJudge.Domain.AggregateRoots.Manager.WitnessEvents;

public class Witness
{
    public static event EventHandler<WitnessEventBase> Events;
    public static void Raise(WitnessEventBase witnessEvent)
    {
        Events?.Invoke(null, witnessEvent);
    }

    public static event EventHandler StateChanged;
    public static void RaiseStateChanged()
    {
        StateChanged?.Invoke(null, EventArgs.Empty);
    }

    public static event EventHandler StartlistChanged;
    public static void RaiseStartlistChanged()
    {
        StateChanged!.Invoke(null, EventArgs.Empty);
    }
}
