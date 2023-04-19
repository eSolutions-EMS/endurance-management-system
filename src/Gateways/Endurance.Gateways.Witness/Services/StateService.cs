using Endurance.Gateways.Witness.Models;

namespace Endurance.Gateways.Witness.Services;

public class StateService : IState
{
	public string? ApiHost { get; set; }

	public Dictionary<int, ManualWitnessEvent> WitnessRecords { get; } = new();
}

public interface IState
{
	string? ApiHost { get; }

	Dictionary<int, ManualWitnessEvent> WitnessRecords { get; }
}
