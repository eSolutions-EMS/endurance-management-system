using Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;

namespace EMS.Witness.Components;

public partial class StartlistComponent
{
    public override List<StartlistEntry> State { get; set; } = new List<StartlistEntry>();
}
