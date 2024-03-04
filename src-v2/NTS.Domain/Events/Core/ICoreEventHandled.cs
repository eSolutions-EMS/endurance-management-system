namespace NTS.Domain.Events.Core;

internal interface ICoreEventHandled
{
	bool IsRejected { get; }
	ICoreEvent CoreEvent { get; }
}
