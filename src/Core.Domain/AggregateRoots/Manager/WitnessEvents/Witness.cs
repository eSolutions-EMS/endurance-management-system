using Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using Core.Enums;
using System;
using System.Collections.Generic;

namespace Core.Domain.AggregateRoots.Manager.WitnessEvents;

public class Witness
{
    public static event EventHandler<WitnessEvent> Events;
    public static void Raise(WitnessEvent witnessEvent)
    {
        Events?.Invoke(null, witnessEvent);
    }

    public static event EventHandler StateChanged;
    public static void RaiseStateChanged()
    {
        StateChanged?.Invoke(null, EventArgs.Empty);
    }

    public static event EventHandler<(string number, CollectionAction action)> StartlistChanged;
    public static void RaiseStartlistChanged(string number, CollectionAction action)
    {
        StartlistChanged?.Invoke(null, (number, action));
    }

    public static event EventHandler<(string number, CollectionAction action)> ArrivelistChanged;
    internal static void RaiseArrivelistChanged(string number, CollectionAction action)
    {
        ArrivelistChanged?.Invoke(null, (number, action));
    }
}
