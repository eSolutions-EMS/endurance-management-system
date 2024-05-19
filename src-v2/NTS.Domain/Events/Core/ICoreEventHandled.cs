 namespace NTS.Domain.Events.Core;

// TODO: redesign this. We need to keep a list of all Snapshot evetns and their result (applied, rejected)
internal interface ICoreEventHandled
{
	bool IsRejected { get; }
	Snapshot CoreEvent { get; }
}
