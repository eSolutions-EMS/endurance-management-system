using Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using EMS.Witness.Models;
using System.Collections.ObjectModel;

namespace EMS.Witness.Services;

public class State : IState
{
	public string? ApiHost { get; set; }
	public Dictionary<int, ManualWitnessEvent> WitnessRecords { get; } = new();
	public List<StartModel> Startlist { get; set; } = new();

}

public interface IState
{
	string? ApiHost { get; }
	Dictionary<int, ManualWitnessEvent> WitnessRecords { get; }
	public List<StartModel> Startlist { get; }
}
