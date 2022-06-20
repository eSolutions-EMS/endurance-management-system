using System;

namespace EnduranceJudge.Domain.AggregateRoots.Manager.WitnessEvents;

public class Witness
{
    public static event EventHandler<WitnessEvent> Events;

    public static void Raise(WitnessEvent witnessEvent)
    {
        Events?.Invoke(null, witnessEvent);
    }
}
