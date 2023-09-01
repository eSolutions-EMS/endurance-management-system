using Core.Application.Services;
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
    private readonly IDateService dateService;

    public ParticipantsService(IWitnessState state, IParticipantsClient arrivelistClient, IToaster toaster, IDateService dateService)
    {
        this.state = state;
        this.participantsClient = arrivelistClient;
        this.toaster = toaster;
        this.dateService = dateService;
    }

    public SortedCollection<ParticipantEntry> Participants => this.state.Participants;
    public ObservableCollection<ParticipantEntry> Snapshots { get; } = new();
    public ObservableCollection<ParticipantEntry> Selected { get; } = new();
    public Dictionary<string, List<ParticipantEntry>> History { get; } = new();

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

    public void Sort(bool byNumber = false, bool byDistance = false, bool byName = false)
    {
        if (byNumber)
        {
            this.Participants.Sort((a, b) => int.Parse(a.Number).CompareTo(int.Parse(b.Number)));
        }
        else if (byDistance)
        {
            this.Participants.Sort((a, b) => a.LapDistance.CompareTo(b.LapDistance));
        }
        else if (byName)
        {
            this.Participants.Sort((a, b) => a.Name.CompareTo(b.Name));
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
            var key = $"{this.dateService.FormatTime(DateTime.Now)} - {type}";
            this.History.Add(key, this.Snapshots);
            this.Snapshots.Clear();
        }
    }
    public async Task Resend(string historyKey, WitnessEventType type)
    {
        // TODO: fix type
        var list = this.History[historyKey];
        var result = await this.participantsClient.Save(list);
        if (result.IsSuccessful)
        {
            this.toaster.Add($"{nameof(this.Resend)} Successful", $"Resent '{list.Count}' entries", UiColor.Success, 3);
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
        var existing = this.Snapshots.FirstOrDefault(x => x == entry);
        if (existing != null)
        {
            this.Snapshots.Remove(existing);
        }
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
    Dictionary<string, List<ParticipantEntry>> History { get; }
    Task Load();
    void Sort(bool byNumber = false, bool byDistance = false, bool byName = false);
    void Update(ParticipantEntry entry, CollectionAction action);
    void Select(ParticipantEntry entry);
    void Unselect(ParticipantEntry entry);
    void EditSnapshot(string number, DateTime time);
    void Snapshot(ParticipantEntry entry);
    void RemoveSnapshot(ParticipantEntry entry);
    Task SaveSnaphots(WitnessEventType type);
    Task Resend(string historyKey, WitnessEventType type);
}