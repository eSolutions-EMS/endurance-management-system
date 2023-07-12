using Core.ConventionalServices;
using Core.Domain.AggregateRoots.Manager;
using Core.Domain.AggregateRoots.Manager.Aggregates.ParticipantEntries;
using Core.Enums;
using Core.Models;
using EMS.Witness.Rpc;
using EMS.Witness.Shared.Toasts;

namespace EMS.Witness.Services;

public class ParticipantsService : IParticipantsService
{
    private readonly IWitnessState state;
    private readonly IParticipantsClient participantsClient;
    private readonly IToaster toaster;

    public ParticipantsService(IWitnessState state, IParticipantsClient arrivelistClient, IToaster toaster)
    {
        this.state = state;
        this.participantsClient = arrivelistClient;
        this.toaster = toaster;
    }

    public SortedCollection<ParticipantEntry> Participants => this.state.Participants;
    public ObservableCollection<ParticipantEntry> Snapshots { get; } = new();
    public ObservableCollection<ParticipantEntry> Selected { get; } = new();
    public List<ParticipantEntry> History { get; } = new();

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
        var result = await this.participantsClient.Load();
        if (result.IsSuccessful)
        {
			this.Participants.Clear();
			this.Participants.AddRange(result.Data!);
		}
    }

    public void RemoveSnapshot(ParticipantEntry entry)
    {
        entry.ArriveTime = null;
        this.Snapshots.Remove(entry);
        this.Participants.Add(entry);
    }

    public void Unselect(ParticipantEntry entry)
    {
        this.Selected.Remove(entry);
    }

    public async Task SaveSnaphots(WitnessEventType type)
    {
        foreach (var entry in this.Snapshots)
        {
            entry.Type = type;
        }
        var result = await this.participantsClient.Save(this.Snapshots);
        if (result.IsSuccessful)
        {
            this.History.AddRange(this.Snapshots);
            this.Snapshots.Clear();
        }
    }
    public async Task Save(ParticipantEntry entry)
    {
        var list = new List<ParticipantEntry> { entry };
        var result = await this.participantsClient.Save(list);
        if (result.IsSuccessful)
        {
            this.toaster.Add("Save Successful", $"Entry '{entry.Number}-{entry.Name}' saved successfully", UiColor.Success, 3);
        }
    }

    public void Select(ParticipantEntry entry)
    {
        if (!this.Selected.Contains(entry))
        {
            this.Selected.Add(entry);
        }
    }

    public void Snapshot(ParticipantEntry entry)
    {
        this.Selected.Remove(entry);

        entry.ArriveTime = DateTime.Now;
        this.Snapshots.Add(entry);
    }

    public void Update(ParticipantEntry entry, CollectionAction action)
    {
        this.Participants.Update(entry, action);
    }
}

public interface IParticipantsService : ISingletonService
{
    SortedCollection<ParticipantEntry> Participants { get; }
    ObservableCollection<ParticipantEntry> Selected { get; }
    ObservableCollection<ParticipantEntry> Snapshots { get; }
    List<ParticipantEntry> History { get; }
    public Task Load();
    public void Update(ParticipantEntry entry, CollectionAction action);
    public void Select(ParticipantEntry entry);
    public void Unselect(ParticipantEntry entry);
    public void EditSnapshot(string number, DateTime time);
    public void Snapshot(ParticipantEntry entry);
    public void RemoveSnapshot(ParticipantEntry entry);
    public Task SaveSnaphots(WitnessEventType type);
    Task Save(ParticipantEntry entry);
}