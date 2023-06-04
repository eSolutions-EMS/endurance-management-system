using Core.Domain.AggregateRoots.Manager;
using Core.Domain.AggregateRoots.Manager.Aggregates.Arrivelists;
using Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using Core.Models;

namespace EMS.Witness.Services;

public class WitnessState : Observable, IWitnessState
{
	public string? ApiHost { get; set; }
	public WitnessEventType Type { get; set; } = WitnessEventType.Arrival;
	public Dictionary<string, WitnessEvent> WitnessRecords { get; } = new();
	public Startlist Startlist { get; } = new();
	public SortedCollection<ArrivelistEntry> Arrivelist { get; } = new();
}

public interface IWitnessState
{
	string? ApiHost { get; }
	WitnessEventType Type { get; set; }
	Dictionary<string, WitnessEvent> WitnessRecords { get; }
	public Startlist Startlist { get; }
	public SortedCollection<ArrivelistEntry> Arrivelist { get; }
}
