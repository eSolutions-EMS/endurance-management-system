using Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using EMS.Witness.Models;

namespace EMS.Witness.Services;

public class State : IState
{
	public static EventHandler<object>? StateChanged;

	public string? ApiHost { get; set; }
	public Dictionary<int, ManualWitnessEvent> WitnessRecords { get; } = new();
	public List<StartModel> Startlist { get; set; } = new();

	public void RaiseStateChanged(object obj)
	{
		StateChanged?.Invoke(this, obj);
	}
}

public interface IState
{
	string? ApiHost { get; }
	Dictionary<int, ManualWitnessEvent> WitnessRecords { get; }
	public List<StartModel> Startlist { get; }
}
