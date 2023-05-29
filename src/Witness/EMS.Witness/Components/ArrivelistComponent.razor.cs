using Core.Domain.AggregateRoots.Manager.Aggregates.Arrivelists;
using Core.Models;
using EMS.Witness.Rpc;
using EMS.Witness.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace EMS.Witness.Components;

public partial class ArrivelistComponent
{
    public override SortedCollection<ArrivelistEntry> State { get; set; } = new();

    [Inject]
    private IArrivelistClient Client { get; set; } = null!;
    [Inject]
    private IState AppState { get; set; } = null!;
    private List<ArrivelistEntry> snapshots = new List<ArrivelistEntry>();
    private List<ArrivelistEntry> history = new List<ArrivelistEntry>();

    private async Task Save(MouseEventArgs _)
    {
        try
        {

            await this.Client.Save(this.snapshots);
            this.history.AddRange(this.snapshots);
            this.snapshots.Clear();
            await InvokeAsync(this.StateHasChanged);
        }
        catch (Exception ex)
        {
            ;
        }
    }

    private void Snapshot(ArrivelistEntry entry)
    {
        entry.ArriveTime = DateTime.Now;
        entry.Type = this.AppState.Type;
        this.snapshots.Add(entry);
        this.State.Remove(entry);
    }
}
