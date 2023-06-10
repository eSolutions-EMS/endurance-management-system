using Core.ConventionalServices;
using Core.Domain.AggregateRoots.Manager;
using Core.Domain.AggregateRoots.Manager.Aggregates.Arrivelists;
using Core.Enums;
using Core.Models;
using EMS.Witness.Rpc;
using EMS.Witness.Shared.Toasts;

namespace EMS.Witness.Services;

public class ArrivelistService : IArrivelistService
{
    private readonly IWitnessState state;
    private readonly IArrivelistClient arrivelistClient;
    private readonly IToaster toaster;

    public ArrivelistService(IWitnessState state, IArrivelistClient arrivelistClient, IToaster toaster)
    {
        this.state = state;
        this.arrivelistClient = arrivelistClient;
        this.toaster = toaster;
    }

    public SortedCollection<ArrivelistEntry> Arrivelist => this.state.Arrivelist;
    public ObservableCollection<ArrivelistEntry> Snapshots { get; } = new();
    public ObservableCollection<ArrivelistEntry> Selected { get; } = new();
    public List<ArrivelistEntry> History { get; } = new();

    public void EditSnapshot(string number, DateTime time)
    {
        var entry = this.Snapshots.FirstOrDefault(x => x.Number == number);
        if (entry is null)
        {
            this.toaster.Add("Not found", $"Entry with number '{number}' not found.", UiColor.Warning);
            return;
        }
        entry.ArriveTime = time;
    }

    public async Task Load()
    {
        var result = await this.arrivelistClient.Load();
        if (result.IsSuccessful)
        {
			this.Arrivelist.Clear();
			this.Arrivelist.AddRange(result.Data!);
		}
    }

    public void RemoveSnapshot(ArrivelistEntry entry)
    {
        entry.ArriveTime = null;
        this.Snapshots.Remove(entry);
        this.Arrivelist.Add(entry);
    }

    public void Unselect(ArrivelistEntry entry)
    {
        this.Selected.Remove(entry);
        this.Arrivelist.Add(entry);
    }

    public async Task Save(WitnessEventType type)
    {
        foreach (var entry in this.Snapshots)
        {
            entry.Type = type;
        }
        var result = await this.arrivelistClient.Save(this.Snapshots);
        if (result.IsSuccessful)
        {
            this.History.AddRange(this.Snapshots);
            this.Snapshots.Clear();
        }
    }

    public void Select(ArrivelistEntry entry)
    {
        this.Selected.Add(entry);
        this.Arrivelist.Remove(entry);
    }

    public void Snapshot(ArrivelistEntry entry)
    {
        this.Selected.Remove(entry);

        entry.ArriveTime = DateTime.Now;
        this.Snapshots.Add(entry);
    }

    public void Update(ArrivelistEntry entry, CollectionAction action)
    {
        this.Arrivelist.Update(entry, action);
    }
}

public interface IArrivelistService : ISingletonService
{
    SortedCollection<ArrivelistEntry> Arrivelist { get; }
    ObservableCollection<ArrivelistEntry> Selected { get; }
    ObservableCollection<ArrivelistEntry> Snapshots { get; }
    List<ArrivelistEntry> History { get; }
    public Task Load();
    public void Update(ArrivelistEntry entry, CollectionAction action);
    public void Select(ArrivelistEntry entry);
    public void Unselect(ArrivelistEntry entry);
    public void EditSnapshot(string number, DateTime time);
    public void Snapshot(ArrivelistEntry entry);
    public void RemoveSnapshot(ArrivelistEntry entry);
    public Task Save(WitnessEventType type);
}