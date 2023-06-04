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
    private readonly IState state;
    private readonly IArrivelistClient arrivelistClient;
    private readonly ToasterService toaster;

    public ArrivelistService(IState state, IArrivelistClient arrivelistClient, ToasterService toaster)
    {
        this.state = state;
        this.arrivelistClient = arrivelistClient;
        this.toaster = toaster;
    }

    public WitnessEventType Type
    {
        get => this.state.Type;
        set => this.state.Type = value; 
    }
    public SortedCollection<ArrivelistEntry> Arrivelist => this.state.Arrivelist;
    public ObservableCollection<ArrivelistEntry> Snapshots { get; } = new();
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

    public async Task Save()
    {
        var result = await this.arrivelistClient.Save(this.Snapshots);
        if (result.IsSuccessful)
        {
            this.History.AddRange(this.Snapshots);
            this.Snapshots.Clear();
        }
    }

    public void Snapshot(ArrivelistEntry entry)
    {
        this.Arrivelist.Remove(entry);

        entry.ArriveTime = DateTime.Now;
        entry.Type = this.state.Type;
        this.Snapshots.Add(entry);
    }

    public void Update(ArrivelistEntry entry, CollectionAction action)
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

public interface IArrivelistService : ISingletonService
{
    WitnessEventType Type { get; set; }
    SortedCollection<ArrivelistEntry> Arrivelist { get; }
    ObservableCollection<ArrivelistEntry> Snapshots { get; }
    List<ArrivelistEntry> History { get; }
    public Task Load();
    public void Update(ArrivelistEntry entry, CollectionAction action);
    public void Edit(string number, DateTime time);
    public void Snapshot(ArrivelistEntry entry);
    public void RemoveSnapshot(ArrivelistEntry entry);
    public Task Save();
}