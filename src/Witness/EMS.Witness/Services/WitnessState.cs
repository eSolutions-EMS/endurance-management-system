using Core.Domain.AggregateRoots.Manager.Aggregates.Participants;
using Core.Models;

namespace EMS.Witness.Services;

public class WitnessState : IWitnessState
{
    public static event EventHandler<EventArgs>? StateLoaded;
    public ObservableCollection<ParticipantEntry> ParticipantSnapshots { get; private set; } = new();
    public ObservableCollection<ParticipantEntry> ParticipantSelected { get; private set; } = new();
    public SortedCollection<ParticipantsBatch> ParticipantHistory { get; private set; } = new();

    public void Set(IWitnessState state)
    {
        this.ParticipantSnapshots = state.ParticipantSnapshots;
        this.ParticipantSelected = state.ParticipantSelected;
        this.ParticipantHistory = state.ParticipantHistory;
        StateLoaded?.Invoke(this, new EventArgs());
    }
}

public interface IWitnessState
{
    ObservableCollection<ParticipantEntry> ParticipantSnapshots { get; }
    ObservableCollection<ParticipantEntry> ParticipantSelected { get; }
    SortedCollection<ParticipantsBatch> ParticipantHistory { get; }
}
