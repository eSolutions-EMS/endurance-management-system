using Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;

namespace EMS.Witness.Components;

public partial class StartlistComponent
{
    public StartlistComponent()
    {
        this.State = new();
    }
}
