using Core.Domain.AggregateRoots.Manager;
using Core.Domain.AggregateRoots.Manager.Aggregates.Participants;
using Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using Core.Models;

namespace EMS.Witness.Services;

public class WitnessContext : Observable, IWitnessContext
{
    public event EventHandler<bool>? IsHandshakingEvent;
    public Dictionary<string, WitnessEvent> WitnessRecords { get; } = new();
    public Dictionary<int, Startlist> Startlists { get; set; } = new();
    public SortedCollection<ParticipantEntry> Participants { get; } = new();

    public void RaiseIsHandshakingEvent(bool isHandshaking)
    {
        this.IsHandshakingEvent?.Invoke(null, isHandshaking);
    }
}

public interface IWitnessContext
{
    event EventHandler<bool> IsHandshakingEvent;
    Dictionary<string, WitnessEvent> WitnessRecords { get; }
    Dictionary<int, Startlist> Startlists { get; set; }
    public SortedCollection<ParticipantEntry> Participants { get; }
}
