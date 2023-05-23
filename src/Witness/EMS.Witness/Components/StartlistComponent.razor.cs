using Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;

namespace EMS.Witness.Components;

public partial class StartlistComponent
{
    public StartlistComponent()
    {
	}

	public override List<StartModel> State { get; set; } = new List<StartModel>();
}
