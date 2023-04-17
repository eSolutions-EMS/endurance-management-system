using EnduranceJudge.Domain.AggregateRoots.Manager;

namespace Endurance.Gateways.Witness.Models;

public class ManualWitnessEvent : WitnessEvent
{
	public int Number { get; set; } 
}
