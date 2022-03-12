using System;

namespace EnduranceJudge.Domain.AggregateRoots.Manager.Aggregates.Startlists;

public class StartModel
{
    public int Number { get; internal init; }
    public string Name { get; internal init; }
    public string CountryName { get; internal init; }
    public double Distance { get; internal init; }
    public DateTime StartTime { get; internal init; }
    public bool HasStarted { get; internal init; }
}
