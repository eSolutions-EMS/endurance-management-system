using System;

namespace EnduranceJudge.Domain.AggregateRoots.Manager.Aggregates.Startlists;

public class StartModel
{
    //TODO: Revert to internal set after AIP integration of StartListPage
    public string Number { get; init; }
    public string Name { get; init; }
    public string CountryName { get; init; }
    public double Distance { get; init; }
    public DateTime StartTime { get; init; }
    public bool HasStarted { get; init; }
}
