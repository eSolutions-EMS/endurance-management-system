using System;

namespace Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;

public class StartlistEntry
{
    public string Number { get; set; }
    public string Name { get; set; }
    public string AthleteName { get; set; }
    public string CountryName { get; set; }
    public double Distance { get; set; }
    public DateTime StartTime { get; set; }
    public bool HasStarted { get; set; }
}
