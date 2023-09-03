using Core.Domain.AggregateRoots.Manager;
using Core.Domain.AggregateRoots.Manager.Aggregates.Participants;
using Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using Core.Models;

namespace EMS.Witness.Services;

public class WitnessState : Observable, IWitnessState
{
	public event EventHandler<bool>? IsHandshakingEvent;
	public string? ApiHost { get; set; }
	public Dictionary<string, WitnessEvent> WitnessRecords { get; } = new();
	public SortedCollection<StartlistEntry> Startlist { get; } = new();
	public SortedCollection<ParticipantEntry> Participants { get; } = new();

	public void RaiseIsHandshakingEvent(bool isHandshaking)
	{
		this.IsHandshakingEvent?.Invoke(null, isHandshaking);
	}
}

public interface IWitnessState
{
	event EventHandler<bool> IsHandshakingEvent;
	string? ApiHost { get; }
	Dictionary<string, WitnessEvent> WitnessRecords { get; }
	public SortedCollection<StartlistEntry> Startlist { get; }
	public SortedCollection<ParticipantEntry> Participants { get; }
}
