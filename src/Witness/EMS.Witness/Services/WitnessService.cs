using Core.ConventionalServices;
using Core.Domain.AggregateRoots.Manager.Aggregates.Arrivelists;
using Core.Enums;
using Core.Models;
using EMS.Witness.Rpc;
using EMS.Witness.Shared.Toasts;

namespace EMS.Witness.Services;

public class WitnessService : IWitnessService
{
    private readonly IState state;
    private readonly IArrivelistClient arrivelistClient;
    private readonly ToasterService toaster;

    public WitnessService(IState state, IArrivelistClient arrivelistClient, ToasterService toaster)
    {
        this.state = state;
        this.arrivelistClient = arrivelistClient;
        this.toaster = toaster;
    }

    public SortedCollection<ArrivelistEntry> Arrivelist => this.state.Arrivelist;
    public List<ArrivelistEntry> Snapshots { get; } = new();
    public List<ArrivelistEntry> History { get; } = new();
        
    public void Edit(string number, DateTime time)
    {
        var entry = this.Snapshots.FirstOrDefault(x => x.Number == number);
        if (entry is null)
        {
            var toast = new Toast("Not found", $"Entry with number '{number}' not found.", UiColor.Warning, 15);
            this.toaster.Add(toast);
            return;
        }
        entry.ArriveTime = time;
    }

    public void Load(IEnumerable<ArrivelistEntry> entries)
    {
        this.Arrivelist.Clear();
        this.Arrivelist.AddRange(entries);
    }

    public async Task Save()
    {
        await this.arrivelistClient.Save(this.Snapshots);
        this.History.AddRange(this.Snapshots);
        this.Snapshots.Clear();
    }

    public void Snapshot(ArrivelistEntry entry)
    {
        this.Arrivelist.Remove(entry);

        entry.ArriveTime = DateTime.Now;
        entry.Type = this.state.Type;
        this.Snapshots.Add(entry);
    }

    public void UpdateArrivelist(ArrivelistEntry entry, CollectionAction action)
    {
        var existing = this.Snapshots.FirstOrDefault(x => x.Number == entry.Number);
        if (existing is not null)
        {
            this.Snapshots.Remove(existing);
        }
        if (action == CollectionAction.AddOrUpdate)
        {
            this.Snapshots.Add(entry);
        }
    }
}

public interface IWitnessService : ISingletonService
{
    SortedCollection<ArrivelistEntry> Arrivelist { get; }
    List<ArrivelistEntry> Snapshots { get; }
    List<ArrivelistEntry> History { get; }
    public void Load(IEnumerable<ArrivelistEntry> entries);
    public void UpdateArrivelist(ArrivelistEntry entry, CollectionAction action);
    public void Edit(string number, DateTime time);
    public void Snapshot(ArrivelistEntry entry);
    public Task Save();
}