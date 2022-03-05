using System;

namespace EnduranceJudge.Domain.AggregateRoots.Manager.Aggregates.Startlists;

public class StartModel
{
    public int Number { get; internal set; }
    public string Name { get; internal set; }
    public string CountryName { get; internal set; }
    public double Distance { get; internal set; }
    public DateTime StartTime { get; internal set; }
    public bool HasStarted { get; internal set; }
}
