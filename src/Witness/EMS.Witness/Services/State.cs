using Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using Core.Models;
using EMS.Witness.Models;

namespace EMS.Witness.Services;

public class State : Observable, IState
{
	public string? ApiHost { get; set; }
	public Dictionary<int, ManualWitnessEvent> WitnessRecords { get; } = new();
	public ObservableCollection<StartModel> Startlist { get; } = new();
}

public interface IState
{
	string? ApiHost { get; }
	Dictionary<int, ManualWitnessEvent> WitnessRecords { get; }
	public ObservableCollection<StartModel> Startlist { get; }
}
