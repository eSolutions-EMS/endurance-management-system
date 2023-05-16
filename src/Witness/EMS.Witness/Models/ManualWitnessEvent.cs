using EMS.Core.Domain.AggregateRoots.Manager;

namespace EMS.Witness.Models;

public class ManualWitnessEvent : WitnessEvent
{
	public int Number { get; set; } 
}
