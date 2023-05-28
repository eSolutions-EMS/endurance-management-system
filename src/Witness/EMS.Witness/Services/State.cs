using Core.Domain.AggregateRoots.Manager;
using Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using Core.Models;

namespace EMS.Witness.Services;

public class State : Observable, IState
{
	public string? ApiHost { get; set; }
	public Dictionary<string, WitnessEvent> WitnessRecords { get; } = new();
	public ObservableCollection<StartModel> Startlist { get; } = new();
}

public interface IState
{
	string? ApiHost { get; }
	Dictionary<string, WitnessEvent> WitnessRecords { get; }
	public ObservableCollection<StartModel> Startlist { get; }
}
